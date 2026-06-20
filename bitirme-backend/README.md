# AI Employee Development System — Backend

> **Bitirme Projesi** — Yapay zekâ destekli çalışan gelişim ve aksiyon planı sistemi (Backend)

Bu repository, üniversite **bitirme projesi** kapsamında geliştirilen "AI Employee Development System" uygulamasının **backend (sunucu tarafı) servisini** içerir. Sistem, çalışanların 360° yetkinlik değerlendirmelerini toplar, bir makine öğrenmesi modeline besleyerek kişiye özel gelişim aksiyonları önerir ve bu önerileri düzenlenebilir aksiyon planlarına ve çalışanlara atanan görevlere dönüştürür.

---

## 🎯 Proje Neden Yapıldı?

Kurumlarda çalışan gelişimi genellikle manuel, sübjektif ve zaman alıcı bir süreçtir. Yöneticiler, değerlendirme sonuçlarına bakarak hangi gelişim adımlarının atılması gerektiğine çoğunlukla deneyime dayalı, standart olmayan kararlar verir. Bu projenin amacı bu süreci **veriye dayalı ve yarı-otonom (agentic AI)** hâle getirmektir:

- **360° değerlendirme** ile bir çalışan hakkında çok kaynaklı (kendisi, yöneticisi, eş düzey ve astları) objektif veri toplanır.
- Toplanan yetkinlik skorları bir **ML modeline** beslenerek çalışana en uygun gelişim aksiyonları tahmin edilir.
- Tahmin edilen ham öneriler, bir yönetici tarafından **düzenlenebilir bir aksiyon planına** dönüştürülür.
- Onaylanan plan, çalışana **atanabilir görevler** hâline gelir ve PDF olarak dışa aktarılabilir.

Böylece çalışan gelişimi; tutarlı, izlenebilir, kişiselleştirilmiş ve büyük ölçüde otomatik bir akışa kavuşur.

### Çekirdek İş Akışı

```text
AiPredictedAction   →   ActionPlanItem   →   EmployeeTask
(ML'in ham önerisi)     (düzenlenmiş plan)    (çalışana atanan görev)
```

---

## 🧩 Teknik Mimari

Proje, sorumlulukların net ayrıştırıldığı **katmanlı (Clean Architecture benzeri) bir yapı** kullanır:

```text
src/
  BitirmeBackend.Api            — Controllers, Middleware, Extensions (sunum katmanı)
  BitirmeBackend.Application    — Servisler, Interface'ler, iş kuralları, Validator'lar
  BitirmeBackend.Domain         — Entity'ler, Enum'lar, ortak çekirdek
  BitirmeBackend.Infrastructure — Persistence (EF Core), Repository'ler, ML Client, Auth
  BitirmeBackend.Contracts      — Request/Response DTO'ları
tests/
  BitirmeBackend.Tests          — xUnit birim/entegrasyon testleri
```

**Tasarım ilkeleri:**

- Controller'lar iş mantığı içermez; tüm iş kuralları **Application** katmanındaki servislerde yaşar.
- DTO ve Entity birbirine karıştırılmaz; dış dünya yalnızca `Contracts` üzerinden iletişim kurar.
- Veritabanı hazır değilken çalışabilmek için **Mock Repository** desteği vardır.
- ML servisine yapılan çağrılar **Circuit Breaker + timeout** ile korunur (retry kullanılmaz, böylece yük altındaki ML servisi daha fazla yorulmaz).

### Öne Çıkan Özellikler

- **JWT + Refresh Token** kimlik doğrulama; refresh token'lar DB'de ham değil **SHA256 hash** olarak saklanır ve **token rotation** uygulanır.
- **360° çok değerlendiricili** değerlendirme mimarisi; kategori bazlı (Self/Manager/Peer/Subordinate) konsolidasyon.
- **ML entegrasyonu** ile kişiye özel aksiyon tahmini (FastAPI servisi).
- **DeepSeek LLM** ile aksiyon planı için doğal dilde özet üretimi.
- **QuestPDF** ile anlık (DB'de saklanmadan) PDF rapor üretimi.
- **Idempotent** plan gönderimi ve **transaction** ile veri tutarlılığı.

---

## 🛠️ Kullanılan Teknolojiler & Kütüphaneler

| Alan | Teknoloji |
|------|-----------|
| Framework | **ASP.NET Core** (Web API, `net10.0` hedef framework) |
| Veritabanı | **PostgreSQL** |
| ORM | **Entity Framework Core** + **Npgsql** |
| Kimlik Doğrulama | **JWT Bearer** (`Microsoft.AspNetCore.Authentication.JwtBearer`) + Refresh Token |
| Loglama | **Serilog** (`Serilog.AspNetCore`, Console sink) |
| Dayanıklılık | **Polly** + `Microsoft.Extensions.Http.Resilience` (Circuit Breaker) |
| PDF Üretimi | **QuestPDF** |
| Doğrulama | **FluentValidation** |
| API Dokümantasyon | **Swashbuckle / Swagger** |
| Test | **xUnit** + **Moq** + **FluentAssertions** |
| ML Servisi | **FastAPI** (Python, ayrı servis) |
| LLM Özet | **DeepSeek** API |

---

## 🚀 Nasıl Çalıştırılır?

### Gereksinimler

- [.NET SDK](https://dotnet.microsoft.com/download) (`net10.0` destekli sürüm)
- **PostgreSQL** (veya geliştirme için Mock Repository modu)
- **Python + FastAPI** ML servisi (aksiyon tahmini için, `http://localhost:8001`)

### Adımlar

1. **Yapılandırmayı hazırla:** `appsettings.example.json` dosyasını kopyalayıp `src/BitirmeBackend.Api/appsettings.Development.json` olarak kaydet ve değerleri doldur:
   - `ConnectionStrings:DefaultConnection` — PostgreSQL bağlantı dizesi
   - `Jwt:Secret` — en az 32 karakterlik gizli anahtar
   - `MlService:BaseUrl` — ML servisinin adresi (varsayılan `http://localhost:8001`)
   - `DeepSeek:ApiKey` — LLM özet özelliği için (opsiyonel)

   > ⚠️ `appsettings.Development.json`, JWT secret ve connection string **asla commit edilmez**.

2. **Bağımlılıkları yükle:**
   ```bash
   dotnet restore
   ```

3. **(Opsiyonel) Veritabanı migration'larını uygula:**
   ```bash
   dotnet ef database update --project src/BitirmeBackend.Infrastructure --startup-project src/BitirmeBackend.Api
   ```

4. **Uygulamayı çalıştır:**
   ```bash
   dotnet run --project src/BitirmeBackend.Api
   ```

5. **API dokümantasyonu:** Uygulama ayağa kalktıktan sonra **Swagger UI** üzerinden endpoint'leri inceleyebilirsin.

### ML Servisi

Aksiyon tahmini için FastAPI servisi ayrı çalışmalıdır:

```text
POST http://localhost:8001/predict-actions/top-k?k=13
GET  http://localhost:8001/health
```

Bkz: `../bitirme-ml/ml_action_recommendation_service`

### Demo Kullanıcı

```text
buse.demir@demo.com / Demo1234!   (tamamlanmış örnek değerlendirme)
```

---

## 🧪 Testler

```bash
dotnet test
```

---

## 👥 Ekip & Frontend

Bu proje bir **ekip çalışmasının** ürünüdür. Bu repository yalnızca **backend** servisini içerir.

> 🔗 **Frontend (kullanıcı arayüzü) için:** Projenin önyüz tarafını inceleyebileceğiniz repository:
> **https://github.com/yunus103/ai-employee-development-system.git**

---

## 📌 Notlar

- PDF dosyaları DB'de veya dosya sisteminde saklanmaz; her istekte anlık üretilir.
- ML çağrısından önce mutlaka `IsHealthyAsync()` ile sağlık kontrolü yapılır.
- Enum alanları PostgreSQL'de string olarak saklanır; silmeler soft-delete (`IsDeleted`) ile yapılır.
