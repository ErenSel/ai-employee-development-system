# Backend Entegrasyon Kontrol Raporu

> **Tarih:** 2026-06-14  
> **Dal:** master  
> **Son commit:** 9ba77b7 fix: allow scoped employee survey access  
> **Kapsam:** BACKEND_INTEGRATION_CHECKLIST.md'deki tüm maddeler mevcut kaynak kodla karşılaştırılmıştır.  
> **Not:** Bu rapor salt okuma (read-only) analizidir; hiçbir kod değişikliği yapılmamıştır.

---

## Authentication / Yetkilendirme

### POST /api/auth/login
✅ **MEVCUT**  
E-posta + şifre ile giriş yapar; `accessToken`, `refreshToken`, `accessTokenExpiresAt`, `userId`, `fullName`, `email`, `role`, `employeeId` döner. Demo şifreleri (`Demo1234!`, `Hr1234!`, `Admin1234!`) seed datası aracılığıyla desteklenmektedir.

---

### POST /api/auth/refresh
✅ **MEVCUT**  
Geçerli `refreshToken` ile yeni `accessToken + refreshToken` çifti döner. Token rotasyonu uygulanmıştır; eski token revoke edilir.

---

### POST /api/auth/logout
⚠️ **KISMİ**  
Endpoint mevcut ve `refreshToken` revoke işlemi çalışıyor. **Ancak kritik bir sorun var:** Endpoint `[Authorize]` attribute'u ile korunuyor (`AuthController.cs:32`). Bu nedenle, `accessToken`'ın süresi dolduğunda istek `401 Unauthorized` hatası verir. Checklist açıkça "accessToken süresi dolsa bile çıkış başarıyla tamamlanmalı, 401 dönülmemeli" demektedir. Frontend sonsuz çıkış döngüsüne girebilir.

**Gerçekte nasıl çalışıyor:** `[Authorize]` + `[HttpPost("logout")]` → süresi dolmuş token 401 döner.  
**Beklenen:** `[AllowAnonymous]` veya özel middleware ile access token doğrulaması atlanarak sadece `refreshToken` revoke edilmeli.

---

### GET /api/auth/me
✅ **MEVCUT**  
`userId`, `fullName`, `email`, `role`, `employeeId` (nullable), `isActive` döner. HR ve Admin kullanıcılarında `employeeId` `null` olarak gelir (User.EmployeeId nullable int).

---

## Employee / Çalışan Endpoint'leri

### GET /api/employees
⚠️ **KISMİ**  
Sayfalanmış liste döner, Manager filtresi de çalışır (`CurrentUserRole == "Manager"` ise managerFilter devreye girer). **Ancak** `EmployeeListItemDto` içinde `performanceScore` alanı **yoktur** (`EmployeeListItemDto.cs` incelendi: Id, EmployeeCode, FullName, Email, Department, JobRole, ManagerId, IsActive). Checklist'te bu "Eksik Alan Düzeltmesi" olarak özellikle belirtilmiştir.

**Gerçekte dönen alanlar:** performanceScore dahil değil.  
**Beklenen:** Her çalışan nesnesinde `performanceScore` bulunmalı.

---

### GET /api/employees/{id}
✅ **MEVCUT**  
`EmployeeDetailDto` ile tam kişisel ve departman bilgileri döner (30+ alan). Rol tabanlı erişim kontrolü uygulanmış: Employee sadece aktif assignment'ı olduğu çalışanlara `EmployeeBasicInfoDto` ile ulaşabilir; Manager kendi ekibine erişebilir.

---

### POST /api/employees
✅ **MEVCUT**  
`[AdminOnly]` politikası ile korunur. `CreateEmployeeRequest` ile 20+ ML özelliği kabul eder ve FluentValidation ile doğrulanır.

---

### GET /api/employees/{id}/assessments
✅ **MEVCUT**  
Sayfalanmış değerlendirme listesi döner. Manager scope (`EnsureManagerCanAccessEmployeeAsync`) kontrol edilmektedir.

---

### GET /api/employees/{id}/action-plans
✅ **MEVCUT**  
Employee kendi planlarını görebilir (ID eşleşmesi kontrolü), HR/Admin/Manager `HrOrManager` policy ile erişebilir. Manager scope uygulanmıştır.

---

### GET /api/employees/{id}/features
✅ **MEVCUT**  
360° konsolidasyon dahil tam özellik vektörü (29 alan) döner. Eksik alan varsa 400 Bad Request + `missingFeatures` listesi döner. Manager scope kontrol edilmektedir.

---

## Assessment / Değerlendirme Endpoint'leri

### POST /api/assessments
⚠️ **KISMİ**  
İki kritik iş kuralından biri eksik:

- ✅ Aktif tamamlanmamış değerlendirme varsa 400 döner.
- ✅ Aktif tamamlanmamış gelişim planı varsa 400 döner.
- ✅ **Self** ataması otomatik oluşturulur (`AssessmentService.cs:59`).
- ❌ **Manager** ataması otomatik **oluşturulmaz**. Checklist "eğer managerId tanımlıysa Manager atama kaydı otomatik eklenmeli" demektedir. Mevcut kod sadece Self ataması yapıyor.

**Gerçekte nasıl çalışıyor:** Sadece Self assignment otomatik ekleniyor; Manager için HR'ın manuel olarak `POST /assignments` çağırması gerekiyor.  
**Beklenen:** `Employee.ManagerId != null` ise Manager için de otomatik assignment oluşturulmalı.

---

### GET /api/assessments/{id}
✅ **MEVCUT**  
Değerlendirme detayı, durumu (`Draft`, `Completed`) ve ilişkili alanlar döner. Manager scope kontrolü mevcut.

---

### PUT /api/assessments/{id}/complete
⚠️ **KISMİ**  
Birkaç sorun var:

1. **Yetki hatası:** Endpoint `[HrOrManager]` policy kullanıyor, bu da Admin + HR + **Manager**'ı kapsıyor. Ancak checklist matrisine göre bu bypass işlemi **sadece Admin ve HR** yapabilmeli; Manager yapamamalı.
2. **overallScore hesaplanmıyor:** Manuel kapanış kodu (`AssessmentService.cs:73-88`) statüyü `Completed` yapar ama mevcut puanlardan `overallScore` hesaplamaz. Bu hesaplama sadece otomatik kapanış yolunda (`TryAutoCompleteAssessmentAsync`) yapılmaktadır. Checklist "mevcut puanların ortalamasını alarak overallScore güncellenmeli" demektedir.

**Gerçekte nasıl çalışıyor:** Status → Completed, overallScore null kalır. Manager da çağırabilir.  
**Beklenen:** overallScore mevcut skorlardan hesaplanmalı; Manager bu endpoint'i çağıramamalı (403 dönmeli).

---

### GET /api/assessments/{id}/scores
✅ **MEVCUT**  
Employee kendi evaluator ID'sine ait skorları görebilir; HR/Admin/Manager tüm skorlara erişebilir. Yetki kontrolleri uygulanmıştır.

---

### POST /api/assessments/{id}/scores (tekil skor)
⚠️ **KISMİ**  
Upsert mantığı çalışıyor, 0-5 aralığı doğrulanıyor, auto-complete tetikleniyor. **Ancak HR/Admin proxy modu çalışmıyor:** `AssessmentService.UpsertAssessmentScoreAsync` her kullanıcı için `GetAssignmentAsync()` kontrolü yapar ve atama bulunamazsa `"Bu değerlendirme için atama bulunamadı"` hatası fırlatır (`AssessmentService.cs:115-116`). Checklist bu hatanın HR/Admin için **fırlatılmaması** gerektiğini açıkça belirtiyor.

**Gerçekte nasıl çalışıyor:** HR/Admin da assignment olmadan skor giremez; tüm kullanıcılar aynı assignment kontrolüne tabi.  
**Beklenen:** HR/Admin rolleri için assignment kontrolü atlanmalı; proxy modunda skor yazılabilmeli.

---

### POST /api/assessments/{id}/scores/bulk (toplu skor)
⚠️ **KISMİ**  
13 yetkinlik bir seferde gönderilebilir, assignment otomatik `IsCompleted=true` yapılır, assessment otomatik `Completed` olur. **Ancak aynı HR/Admin proxy sorunu geçerli:** `BulkUpsertScoresAsync` da `GetAssignmentAsync()` kontrolü yapıyor (`AssessmentService.cs:265-266`). HR, atama olmayan bir çalışan adına toplu skor girişi yapamıyor.

---

### POST /api/assessments/{id}/assignments
✅ **MEVCUT**  
Peer/Subordinate ataması oluşturulur. Self ve Manager için tekrarlı atama kontrolü uygulanmış. `[HrOrManager]` ile korunur.

---

### GET /api/assessments/{id}/assignments
✅ **MEVCUT**  
Tüm değerlendiriciler, adları ve `isCompleted` durumlarıyla listelenir.

---

## Action Plan Endpoint'leri

### POST /api/action-plans/generate
⚠️ **KISMİ**  
ML entegrasyonu büyük ölçüde çalışıyor: assessment kontrolü, özellik vektörü derleme, ML çağrısı, action catalog eşleştirme, transaction içinde plan oluşturma, circuit breaker hepsi mevcut. **Ancak kritik bir sorun var:**

**Priority her zaman `Medium` olarak sabitlenmiş** (`ActionPlanService.cs:183`: `Priority = PriorityLevel.Medium`). ML modeli öneri dönerken probability değeri geliyor ama priority bilgisi gelmiyor. `MlRecommendedActionDto` sadece `Code`, `Probability`, `Selected` alanlarını içeriyor; `priority` alanı yok. Checklist "zayıf yetkinliklerin derecesine göre öncelik dinamik olarak belirlenmeli (Low/Medium/High), ML çıktısındaki öncelik verisi item'lara yazılmalı" demektedir.

Ayrıca: `appsettings.json` ML URL'i `http://localhost:8001` olarak tanımlı. Checklist `https://bitirme-ml.onrender.com` referansı veriyor. URL appsettings'ten okunuyor (hardcode yok) ancak üretim ortamında güncellenmesi gerekiyor.

---

### GET /api/action-plans/{id}
✅ **MEVCUT**  
Plan detayı ve maddeleri döner. Maddeler tamamlanmış olanlar sona, önceliğe göre sıralanır. Manager scope kontrolü mevcut.

---

### PUT /api/action-plans/{id}/items/{itemId}
✅ **MEVCUT**  
Başlık, açıklama, öncelik, bitiş tarihi, sıra güncellenebilir. AI kaynaklı item → `EditedAI` geçişi uygulanmış. Sadece Draft/Edited plan düzenlenebilir.

---

### POST /api/action-plans/{id}/items
✅ **MEVCUT**  
Manuel madde ekleme; Source=Manual olarak işaretlenir. Plan Draft → Edited geçişini tetikler.

---

### DELETE /api/action-plans/{id}/items/{itemId}
✅ **MEVCUT**  
Soft delete (IsDeleted=true) ile silme yapılır. Plan Draft → Edited geçişini tetikler.

---

### POST /api/action-plans/{id}/approve
✅ **MEVCUT**  
Draft veya Edited plan → Approved geçişi. Boş plan onaylanamaz. İdempotent (zaten Approved ise mevcut state döner).

---

### POST /api/action-plans/{id}/send
✅ **MEVCUT**  
Approved → Sent geçişi. Her aktif madde için `EmployeeTask` (Status=Assigned) oluşturur. İdempotent (task zaten varsa yenisi oluşturulmaz). Transaction içinde çalışır.

---

### POST /api/action-plans/{id}/cancel
✅ **MEVCUT**  
Draft veya Edited planı iptal eder. Approved ve Sent planlar iptal edilemez (iş mantığı gereği). Completed plan da iptal edilemez. İdempotent.

---

### GET /api/action-plans/{id}/export-pdf
⚠️ **KISMİ**  
PDF oluşturma büyük ölçüde çalışıyor:
- ✅ Tüm aktif maddeler listeleniyor.
- ✅ Öncelik sıralaması: High → Medium → Low (tamamlananlar sona).
- ✅ Türkçe karakter desteği: `FontFamily("Noto Sans")` kullanılıyor.
- ✅ Dosya disk'te saklanmıyor, stream olarak dönüyor.
- ✅ `Content-Disposition: attachment; filename=aksiyon-plani-{id}.pdf` header'ı ekleniyor.
- ⚠️ Header'da **EmployeeCode** ve **ManagerName** bilgisi yok (sadece FullName, Department, JobRole, CycleName, tarihler var).
- ❌ **Çalışan kendi gelişim planının PDF'ini indiremiyor.** Endpoint `[HrOrManager]` policy kullanıyor; Employee rolü erişemiyor. Checklist "ilgili planın sahibi olan çalışan da erişebilmeli" demektedir.

---

## Task / Görev Endpoint'leri

### GET /api/tasks/my
⚠️ **KISMİ**  
Sayfalanmış görev listesi döner. Ancak `EmployeeTaskDto` ve `EmployeeTaskService.ToDto()` incelendiğinde üç kritik alan **eksik**:

1. ❌ **`actionPlanId`** — Çalışanın PDF indirmesi için plan ID'si yok. Yalnızca `actionPlanItemId` dönüyor.
2. ❌ **`resource`** — ActionPlanItem.Resource alanı mevcut ama ToDto() map etmiyor.
3. ❌ **`deliveryType`** — ActionPlanItem.DeliveryType alanı mevcut ama ToDto() map etmiyor.

`EmployeeTaskDto.cs` dosyasında bu alanlar tanımlı değil.

---

### GET /api/tasks/my-surveys
✅ **MEVCUT**  
Aktif (tamamlanmamış) 360° anket atamalarını `MySurveyDto[]` olarak listeler. `assessmentId`, `employeeName`, `cycleName`, `evaluatorType`, `competencyCount` alanlarını içerir. Sadece `Draft` veya `Completed` durumdaki değerlendirmeler dahil edilir.

---

### PUT /api/tasks/{id}/status
🔄 **FARKLI**  
Auto-complete, validasyon ve durum geçişleri çalışıyor. Tüm görevler Completed olduğunda ActionPlan otomatik Completed olur. **Ancak durum isimleri farklı:**

Checklist: `Pending → InProgress → Completed`  
Gerçek uygulama: `Assigned → InProgress → Completed → Cancelled`

İlk durum adı `Pending` değil `Assigned`'dır (`EmployeeTaskStatus.cs`). Frontend bu durum adını `"Assigned"` olarak beklemelidir.

---

## Genel İş Kuralları ve Sistem Davranışı

### ML Servis Hata Toleransı (Circuit Breaker)
✅ **MEVCUT**  
Production ortamında Polly circuit breaker uygulanmış. `503 ServiceUnavailable` durumu yakalanıyor. Health check (`IsHealthyAsync`) her ML çağrısından önce yapılıyor. Başarısız run DB'ye kaydedilip transaction commit ediliyor.

### HR/Admin Proxy Skor Girişi
⚠️ **KISMİ** (yukarıdaki skor endpoint'lerinde de belirtildi)  
HR ve Admin rolleri `HrOrManager` policy üzerinden skor endpoint'lerine erişebiliyor. Ancak `AssessmentService` assignment varlığını zorunlu kılıyor; atama olmayan bir evaluator adına skor girilemez. Checklist bu kısıtlamanın HR/Admin için devre dışı bırakılması gerektiğini belirtmektedir.

### Manager Bypass (Force-Complete) Yetkisi
⚠️ **KISMİ**  
`PUT /api/assessments/{id}/complete` endpoint'i `HrOrManager` policy ile korunuyor; bu da Manager rolünün de bu endpoint'i çağırabildiği anlamına geliyor. Checklist matrisine göre Manager bu yetkiye sahip olmamalı.

### Kanban — Otomatik Plan Tamamlama
✅ **MEVCUT**  
`EmployeeTaskService.UpdateTaskStatusAsync()` metodunda tüm görevler Completed olduğunda ActionPlan otomatik Completed yapılır.

### Assessment Auto-Complete
✅ **MEVCUT**  
`TryAutoCompleteAssessmentAsync()` — tüm assignment'lar IsCompleted=true olduğunda Assessment otomatik Completed yapılır ve overallScore hesaplanır.

---

## ÖZET

| Kategori | Sayı |
|----------|------|
| ✅ MEVCUT | 21 |
| ⚠️ KISMİ | 11 |
| 🔄 FARKLI | 1 |
| ❌ EKSİK | 0 |

---

## Öncelikli Yapılması Gerekenler (Frontend Entegrasyonunu Engelleyen Maddeler)

Aşağıdaki maddeler frontend entegrasyonunda doğrudan hata üretecek veya belirli özelliklerin hiç çalışmamasına yol açacak nitelikte olup öncelikle giderilmelidir:

1. **🔴 POST /api/auth/logout — Süresi Dolmuş Token ile 401**  
   `[Authorize]` attribute kaldırılıp logout refresh-token bazlı yapılmalı. Frontend'in yenileme döngüsünü kırar.

2. **🔴 GET /api/tasks/my — Eksik DTO Alanları**  
   `EmployeeTaskDto`'ya `actionPlanId`, `resource`, `deliveryType` eklenmeli; `ToDto()` mapper güncellenmeli. PDF indirme ve görev detay linkleri çalışmayacak.

3. **🔴 POST /api/assessments/{id}/scores & /bulk — HR/Admin Proxy Modu Çalışmıyor**  
   `UpsertAssessmentScoreAsync` ve `BulkUpsertScoresAsync` içinde, isteği yapan kullanıcı HR veya Admin ise assignment kontrolü atlanmalı. HR panelindeki proxy veri girişi hiç çalışmayacak.

4. **🟠 GET /api/employees — EmployeeListItemDto'da performanceScore Eksik**  
   `EmployeeListItemDto`'ya `performanceScore` alanı eklenmeli ve servis mapping'i güncellenmeli. UI'da tüm puan sütunu boş görünüyor.

5. **🟠 POST /api/assessments — Manager Ataması Otomatik Oluşturulmuyor**  
   Assessment oluşturulduğunda `Employee.ManagerId != null` ise Manager assignment otomatik eklenmelidir. Yönetici anketi listesinde görünmeyecek.

6. **🟠 POST /api/action-plans/generate — Priority Her Zaman Medium**  
   ML'den dönen probability verisine göre veya yetkinlik skorlarının düşüklüğüne göre dinamik öncelik atanmalı. Tüm action items "Orta" öncelikle oluşturulacak, raporlama bozulacak.

7. **🟠 GET /api/action-plans/{id}/export-pdf — Çalışan Kendi PDF'ini İndiremiyor**  
   Endpoint'e `employeeId` eşleşmesi kontrolü eklenerek çalışanın kendi planına ait PDF'e erişimi açılmalı. Çalışan kendi gelişim planını indiremiyor.

8. **🟡 PUT /api/assessments/{id}/complete — overallScore Hesaplanmıyor**  
   Manuel bypass sırasında mevcut skorlardan `overallScore` hesaplanmalı. Şu an null kalıyor.

9. **🟡 PUT /api/assessments/{id}/complete — Manager Erişimi**  
   `HrOrManager` → `HrOnly` (veya eşdeğeri) policy değiştirilmeli; Manager force-complete yapamamalı.

10. **🟡 PUT /api/tasks/{id}/status — Durum Adı Farkı**  
    Frontend `"Pending"` durumu bekliyorsa `"Assigned"` adını bilmesi veya backend'in `Pending` olarak rename etmesi gerekiyor.

---

## Frontend'in Hemen Bağlanabileceği Endpoint'ler

Aşağıdaki endpoint'ler herhangi bir backend değişikliği gerektirmeksizin hemen entegre edilebilir:

| Endpoint | Notlar |
|----------|--------|
| `POST /api/auth/login` | Tam çalışıyor |
| `POST /api/auth/refresh` | Tam çalışıyor |
| `GET /api/auth/me` | Tam çalışıyor |
| `GET /api/employees/{id}` | Tam çalışıyor |
| `POST /api/employees` | Admin yetkisi gerekli |
| `GET /api/employees/{id}/assessments` | Tam çalışıyor |
| `GET /api/employees/{id}/action-plans` | Çalışan + HR/Manager erişimi |
| `GET /api/employees/{id}/features` | Eksik alan validasyonu dahil |
| `GET /api/assessments/{id}` | Tam çalışıyor |
| `GET /api/assessments/{id}/scores` | Tam çalışıyor |
| `POST /api/assessments/{id}/assignments` | Tam çalışıyor |
| `GET /api/assessments/{id}/assignments` | Tam çalışıyor |
| `GET /api/action-plans/{id}` | Resource/DeliveryType burada mevcut |
| `PUT /api/action-plans/{id}/items/{itemId}` | Tam çalışıyor |
| `POST /api/action-plans/{id}/items` | Tam çalışıyor |
| `DELETE /api/action-plans/{id}/items/{itemId}` | Soft delete çalışıyor |
| `POST /api/action-plans/{id}/approve` | Tam çalışıyor |
| `POST /api/action-plans/{id}/send` | EmployeeTask oluşturma dahil |
| `POST /api/action-plans/{id}/cancel` | Draft/Edited için çalışıyor |
| `GET /api/tasks/my-surveys` | MySurveyDto tam çalışıyor |
| `POST /api/assessments/{id}/scores/bulk` | Atama olan kullanıcılar için çalışıyor |
| `GET /api/health` | Tam çalışıyor |
