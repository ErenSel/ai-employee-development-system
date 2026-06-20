# AI Employee Development System

> **Bitirme Projesi** — Yapay zekâ destekli çalışan gelişim ve aksiyon planı sistemi

Bu repository, üniversite **bitirme projesi** kapsamında geliştirilen "AI Employee Development System" uygulamasının kaynak kodlarını içerir. Sistem, çalışanların 360° yetkinlik değerlendirmelerini toplar, bir makine öğrenmesi modeline besleyerek kişiye özel gelişim aksiyonları önerir ve bu önerileri düzenlenebilir aksiyon planlarına ve çalışanlara atanan görevlere dönüştürür.

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

## 📦 Repository Yapısı

```text
ai-employee-development-system/
├── bitirme-backend/   — ASP.NET Core Web API (sunucu tarafı)
├── bitirme-ml/        — FastAPI ML servisi (aksiyon tahmin modeli)
└── docs/              — Proje dökümantasyonu ve görev listesi
```

> Bu proje bir **ekip çalışmasının** ürünüdür ve birden fazla bileşenden oluşur.

---

## 🧩 Bileşenler

### Backend — `bitirme-backend/`

ASP.NET Core Web API. 360° değerlendirme, kimlik doğrulama, ML entegrasyonu, aksiyon planı yönetimi ve PDF üretiminden sorumludur. Katmanlı (Clean Architecture benzeri) bir mimari kullanır.

**Öne çıkan teknolojiler:** ASP.NET Core (`net10.0`), Entity Framework Core + Npgsql (PostgreSQL), JWT + Refresh Token, Serilog, Polly (Circuit Breaker), QuestPDF, FluentValidation, xUnit/Moq/FluentAssertions.

Ayrıntılı kurulum ve çalıştırma adımları için: [`bitirme-backend/README.md`](./bitirme-backend/README.md)

### ML Servisi — `bitirme-ml/`

FastAPI tabanlı Python servisi. Çalışanın yetkinlik skorlarını girdi alıp en uygun gelişim aksiyonlarını tahmin eder.

```text
POST http://localhost:8001/predict-actions/top-k?k=13
GET  http://localhost:8001/health
```

### Frontend — Ayrı Repository 🔗

Projenin kullanıcı arayüzü (önyüz) ayrı bir repository'de geliştirilmektedir:

> **https://github.com/yunus103/ai-employee-development-system.git**

---

## 🚀 Hızlı Başlangıç

1. **Backend:** Kurulum ve çalıştırma için [`bitirme-backend/README.md`](./bitirme-backend/README.md) dosyasını izleyin.
   ```bash
   dotnet run --project bitirme-backend/src/BitirmeBackend.Api
   ```
2. **ML Servisi:** `bitirme-ml/ml_action_recommendation_service` altındaki FastAPI servisini `http://localhost:8001` üzerinde çalıştırın.
3. **Frontend:** Yukarıdaki frontend repository'sini klonlayıp kendi yönergelerine göre başlatın.

### Demo Kullanıcı

```text
buse.demir@demo.com / Demo1234!   (tamamlanmış örnek değerlendirme)
```

---

## 👥 Ekip

Bu proje bir ekip çalışmasıdır. Backend, ML ve frontend bileşenleri birlikte geliştirilmiştir.
