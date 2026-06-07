# Bitirme Projesi Database Schema Implementation Guide v3

Bu doküman, ASP.NET Core 9.0 + PostgreSQL backend projesinde veritabanı şemasını oluşturacak agentic AI için hazırlanmıştır.

v3 değişiklikleri:
- Mevcut backendin `backend_agentic_ai_task_list_v5.md` dosyasına göre zaten geliştirildiği netleştirildi.
- Bu dokümanın kapsamı yalnızca DB entegrasyonu, EF Core entity/configuration/migration/seed ve gerekli minimum ActionCatalog mapping revizyonları olarak sınırlandırıldı.
- `ActionCatalogs` yapısının backend v5 içindeki eski/sade ActionCatalog tanımını override ettiği açıkça belirtildi.
- `db.sql` dosyasındaki pg_dump/psql meta-komutlarının EF migration içine ham şekilde konulmaması gerektiği eklendi.
- `PerformanceScore` kaynağı netleştirildi: generate action plan akışında öncelik `Assessment.OverallScore`, `Employee.PerformanceScore` ise son/genel skor cache alanı.
- Demo assessment score seed kuralı netleştirildi: basit demo için `EvaluatorType = Manager`, birden fazla evaluator varsa ortalama alınır.
- Duplicate action plan ve duplicate employee task oluşmasını engellemek için partial unique index önerileri eklendi.
- Temel check constraint önerileri eklendi.
- Demo credential uyumluluğu için mevcut backend mock credential kontrolü eklendi.
- Agent prompt mevcut backendin gereksiz yere yeniden yazılmaması için güncellendi.

Amaç:

- `backend_agentic_ai_task_list_v5.md` dosyasındaki backend mimarisine uyumlu PostgreSQL şeması kurmak.
- Ek olarak verilen `db.sql` dosyasındaki `ActionCatalogs` tablosunu ve seed verilerini kullanmak.
- AI action code eşleştirme, action plan üretimi, çalışan task takibi, auth, refresh token ve 360 değerlendirme akışını destekleyen tam DB yapısını hazırlamak.

---

## 1. Kapsam ve Mevcut Backend Durumu

Bu doküman, backend projesi `backend_agentic_ai_task_list_v5.md` dosyasına göre geliştirildikten sonra **DB entegrasyonu eksik kalan kısmı** tamamlatmak için hazırlanmıştır.

Agentic AI bu dokümanı uygularken şu kurallara uymalıdır:

```text
Mevcut backend mimarisi, controller/service/use-case yapısı ve unrelated business logic yeniden yazılmayacaktır.
Sadece DB entegrasyonu için gerekli olan EF Core entity/configuration/AppDbContext/migration/seed/repository implementasyonları tamamlanacaktır.
ActionCatalogs uyumluluğu için gerekiyorsa yalnızca ActionCatalog, ActionPlanItem, IActionCatalogRepository, MockActionCatalogRepository ve ActionPlanService mapping bölümleri revize edilecektir.
```

Özellikle dikkat:

```text
Bu görev bir "backend rewrite" görevi değildir.
Bu görev bir "database integration and schema completion" görevidir.
```

---

## 2. Source of Truth

Agent aşağıdaki üç girdiyi birlikte referans almalıdır:

```text
backend_agentic_ai_task_list_v5.md
```

Bu dosya backend entity, DTO, service, repository, EF Core ve genel mimari ihtiyaçları için ana referanstır. Backend kodunun büyük kısmı bu dosyaya göre zaten geliştirilmiştir; bu doküman mevcut kodun DB entegrasyonu eksiklerini tamamlamak içindir.

```text
db.sql
```

Bu dosya yalnızca `ActionCatalogs` tablosu ve bu tabloya ait seed verileri için referans alınmalıdır.

Önemli:

```text
db.sql tam backend veritabanı değildir.
db.sql yalnızca ActionCatalogs katalog/seed datası olarak kullanılmalıdır.
```

Ek bağlam:

```text
Mevcut backend implementasyonu ile bu doküman arasında çelişki varsa, genel mimari ve endpoint/service akışı için backend_agentic_ai_task_list_v5.md korunur.
ActionCatalogs yapısı, ActionCatalog primary key tipi ve ContentData parsing kuralları için bu database instruction dokümanı önceliklidir.
```

---

## 3. Genel Veritabanı Yaklaşımı

Tek PostgreSQL veritabanı kullanılacaktır.

Önerilen veritabanı adı:

```text
bitirme_db
```

EF Core + Npgsql kullanılacaktır.

Migration stratejisi:

```text
EF Core migration source of truth olmalıdır.
Manual SQL sadece ActionCatalogs seed datasını taşımak için kullanılmalıdır.
```

Agent şu iki yoldan birini seçebilir:

### Tercih Edilen Yol

1. EF Core entity ve configuration sınıflarını oluştur.
2. Migration ile tüm tabloları oluştur.
3. `db.sql` içindeki `ActionCatalogs` seed verilerini EF migration içinde geçerli `INSERT INTO` formatına çevirerek veya ayrı çalıştırılabilir seed SQL scripti ile yükle.

### Alternatif Yol

1. `db.sql` önce çalıştırılır.
2. EF migration buna göre `ActionCatalogs` tablosunu tekrar oluşturmaya çalışmamalıdır.
3. Eksik audit kolonları migration ile eklenir.

Önerilen yol birincisidir. Çünkü backend EF Core ile geliştirilecektir ve migration geçmişinin tek kaynak olması daha güvenlidir.

`db.sql` kullanım uyarısı:

```text
db.sql pg_dump/psql formatında olabilir ve içinde COPY FROM stdin, \restrict, \. gibi psql meta-komutları bulunabilir.
Bu içerik migrationBuilder.Sql(...) içine ham şekilde konulmamalıdır.
Agent, ActionCatalogs seed datasını EF migration için geçerli INSERT INTO komutlarına çevirmeli veya ayrı bir psql seed scripti olarak çalıştırılabilir hale getirmelidir.
```

---

## 4. ActionCatalogs Tablosu İçin Özel Kural

Verilen `db.sql` dosyasında `ActionCatalogs` tablosu şu yapıya sahiptir:

```text
ActionId varchar(50) primary key
TargetCompetency varchar(50)
ActionCategory varchar(50)
ActionType varchar(50)
Difficulty varchar(20)
EstimatedEffortHours integer
MinScore numeric(3,2)
MaxScore numeric(3,2)
ContentData jsonb
```

Bu yapı korunmalıdır.

`ActionId`, AI servisinden gelen `recommendedActions[].code` alanıyla eşleşen ana alandır.

Örnek:

```text
AI response code = DEPT_COMP1_03
ActionCatalogs.ActionId = DEPT_COMP1_03
```

Bu nedenle backend tarafında `ActionCatalog` entity'si klasik `Id int` primary key kullanmamalıdır.

Override kuralı:

```text
backend_agentic_ai_task_list_v5.md içinde geçen eski/sade ActionCatalog tanımı (Code, Title, Description, Category, DefaultPriority, EstimatedDurationDays, IsActive) bu görev kapsamında geçerli değildir.
Bu görevde ActionCatalogs yapısı db.sql ile uyumlu olacak şekilde ActionId string primary key ve ContentData jsonb üzerinden kurulacaktır.
```

Doğru entity yaklaşımı:

```csharp
public class ActionCatalog
{
    public string ActionId { get; set; } = null!;
    public string TargetCompetency { get; set; } = null!;
    public string ActionCategory { get; set; } = null!;
    public string ActionType { get; set; } = null!;
    public string Difficulty { get; set; } = null!;
    public int EstimatedEffortHours { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
    public string ContentData { get; set; } = null!;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
```

EF Core configuration:

```csharp
builder.HasKey(x => x.ActionId);
builder.Property(x => x.ActionId).HasMaxLength(50).IsRequired();
builder.Property(x => x.ContentData).HasColumnType("jsonb").IsRequired();
builder.Property(x => x.MinScore).HasPrecision(3, 2);
builder.Property(x => x.MaxScore).HasPrecision(3, 2);
```

Mevcut backend kodu uyumluluk notu:

```text
Backend'de mevcut ActionCatalog entity'si int Id ile yazılmıştır.
Bu entity string ActionId primary key kullanacak şekilde yeniden yazılmalıdır.
MockActionCatalogRepository da buna göre güncellenmeli; string key ile lookup yapmalıdır.
ActionPlanItem.ActionCatalogId tipi string (varchar(50)) olacaktır.
IActionCatalogRepository.GetByCodesAsync(IEnumerable<string> codes) imzası korunmalıdır.
```

Ek audit kolonları:

```text
IsActive boolean default true
CreatedAt timestamptz default now()
UpdatedAt timestamptz nullable
IsDeleted boolean default false
```

---

## 5. ActionCatalogs İçeriğini Kullanma Kuralı

`ContentData` alanı JSONB'dir.

Entity tarafında başlangıç için `string ContentData` kullanılabilir; EF Core configuration içinde kolon tipi `jsonb` olmalıdır. Backend parsing sırasında `System.Text.Json.JsonDocument.Parse(ContentData)` veya eşdeğer güvenli JSON parsing yöntemi kullanılmalıdır. `ContentData` düz text gibi ele alınmamalıdır.

Core competency action'larında genellikle şu alanları içerir:

```json
{
  "title": "...",
  "resource": "...",
  "description": "...",
  "delivery_type": "..."
}
```

Department veya role bazlı aksiyonlarda `ContentData` nested JSON olabilir.

Örnek yapı:

```json
{
  "Sales & Marketing": {
    "title": "...",
    "resource": "...",
    "delivery_type": "..."
  },
  "Technology": {
    "title": "...",
    "resource": "...",
    "delivery_type": "..."
  }
}
```

Backend mapping kuralı:

1. AI action code ile `ActionCatalogs.ActionId` eşleştir.
2. `ContentData` içinden uygun title/description/resource/delivery_type değerlerini çıkar.
3. Eğer ContentData role veya department bazlı nested yapıdaysa:
   - Önce employee `JobRole` key'i ile eşleşme ara.
   - Bulamazsa employee `Department` key'i ile eşleşme ara.
   - O da yoksa ilk uygun JSON object kullanılabilir.
4. Hiç parse edilemezse fallback kullan.

Fallback:

```text
Title: Gelişim Aksiyonu: {ActionCode}
Description: Bu aksiyon için lütfen yöneticinizle iletişime geçin.
Priority: Medium
```

---

## 6. Zorunlu Tablolar

Aşağıdaki tablolar oluşturulmalıdır.

### 5.1 Roles

Alanlar:

```text
Id int primary key
Name varchar(50) unique not null
Description text nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Seed:

```text
Admin
HR
Manager
Employee
```

---

### 5.2 Users

Alanlar:

```text
Id int primary key
FullName varchar(150) not null
Email varchar(150) unique not null
PasswordHash text not null
RoleId int not null foreign key -> Roles.Id
EmployeeId int nullable foreign key -> Employees.Id
IsActive boolean not null default true
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Not:

```text
User.EmployeeId nullable olmalıdır.
Admin veya bazı HR kullanıcıları çalışan kaydına bağlı olmayabilir.
```

---

### 5.3 RefreshTokens

Alanlar:

```text
Id int primary key
UserId int not null foreign key -> Users.Id
TokenHash text unique not null
ExpiresAt timestamptz not null
IsRevoked boolean not null default false
RevokedAt timestamptz nullable
ReplacedByTokenHash text nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Kural:

```text
Raw refresh token DB'de saklanmamalıdır.
Sadece hash saklanmalıdır.
```

---

### 5.4 Departments

Alanlar:

```text
Id int primary key
Name varchar(100) not null
Code varchar(50) unique not null
Description text nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Seed önerileri:

```text
Technology
Human Resources
Sales & Marketing
Finance & Accounting
Operations
```

---

### 5.5 JobRoles

Alanlar:

```text
Id int primary key
Name varchar(100) not null
DepartmentId int nullable foreign key -> Departments.Id
Description text nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Seed önerileri:

```text
Software Engineer
Data Scientist
HR Specialist
Sales Executive
Operations Manager
Accountant
```

---

### 5.6 Employees

Alanlar:

```text
Id int primary key
EmployeeCode varchar(50) unique not null
FullName varchar(150) not null
Email varchar(150) unique not null
Age int not null
Gender varchar(30) not null
DepartmentId int not null foreign key -> Departments.Id
JobRoleId int not null foreign key -> JobRoles.Id
ManagerId int nullable foreign key -> Employees.Id
Education int not null
EducationField varchar(100) not null
BusinessTravel varchar(100) not null
MaritalStatus varchar(50) not null
DistanceFromHome int not null
EnvironmentSatisfaction decimal(4,2) not null
JobSatisfaction decimal(4,2) not null
WorkLifeBalance decimal(4,2) not null
TotalWorkingYears int not null
YearsAtCompany int not null
YearsInCurrentRole int not null
YearsWithCurrManager int not null
PerformanceScore decimal(4,2) not null
Attrition varchar(10) not null
IsActive boolean not null default true
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Kritik:

```text
PerformanceScore zorunludur.
AI feature request için gereklidir.
```

PerformanceScore kullanım kuralı:

```text
Generate action plan akışında AI'ya gönderilecek PerformanceScore değeri öncelikle Assessments.OverallScore alanından alınmalıdır.
Employees.PerformanceScore alanı çalışanın son/genel performans skorunu cachelemek ve liste/detay ekranlarında hızlı göstermek için tutulabilir.
Seed datada Employees.PerformanceScore ve demo Assessments.OverallScore aynı değerle doldurulmalıdır.
```

---

### 5.7 AssessmentCycles

Alanlar:

```text
Id int primary key
Name varchar(150) not null
StartDate date not null
EndDate date not null
Status varchar(50) not null
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Seed:

```text
Name: 2025 Yılı Q4 Değerlendirmesi
StartDate: 2025-10-01
EndDate: 2025-12-31
Status: Completed
```

Not:

```text
Demo assessment bu cycle'a bağlı olacaktır.
Bu seed olmadan demo assessment CycleId foreign key kısıtını karşılayamaz.
```

---

### 5.8 Assessments

Alanlar:

```text
Id int primary key
EmployeeId int not null foreign key -> Employees.Id
CycleId int not null foreign key -> AssessmentCycles.Id
OverallScore decimal(4,2) not null
Status varchar(50) not null
CreatedByUserId int not null foreign key -> Users.Id
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Kural:

```text
AI action plan üretimi Assessment üzerinden başlatılır.
```

---

### 5.9 Competencies

Alanlar:

```text
Id int primary key
Code varchar(100) unique not null
Name varchar(150) not null
Category varchar(50) not null
Description text nullable
IsActive boolean not null default true
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Seed zorunlu competency code'ları:

```text
Core_Communication
Core_Teamwork
Core_ProblemSolving
Core_Adaptability
Core_Initiative
Core_Accountability
Core_LearningAgility
Core_TimeManagement
Dept_Comp1
Dept_Comp2
Dept_Comp3
Role_Comp1
Role_Comp2
```

---

### 5.10 AssessmentScores

Alanlar:

```text
Id int primary key
AssessmentId int not null foreign key -> Assessments.Id
CompetencyId int not null foreign key -> Competencies.Id
EvaluatorType varchar(50) not null
Score decimal(4,2) not null
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Kural:

```text
Score 0 ile 5 arasında olmalıdır.
```

EvaluatorType değerleri:

```text
Self
Manager
Peer
Subordinate
```

Feature üretim kuralı:

```text
Backend, AssessmentScores kayıtlarını Competencies.Code alanına göre EmployeeFeatureDto alanlarına map etmelidir.
Aynı competency için birden fazla evaluator score varsa ortalama alınabilir.
Demo seed için her competency adına en az bir AssessmentScore kaydı oluşturulmalıdır.
Basit demo için EvaluatorType = Manager kullanılabilir.
```

---

### 5.11 FeedbackComments

Opsiyonel ama oluşturulmalıdır.

Alanlar:

```text
Id int primary key
AssessmentId int not null foreign key -> Assessments.Id
EvaluatorType varchar(50) not null
CommentText text not null
SentimentScore decimal(5,4) nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

---

### 5.12 ModelVersions

Alanlar:

```text
Id int primary key
ModelName varchar(100) not null
Version varchar(100) not null
Description text nullable
MicroF1 decimal(8,6) nullable
MacroF1 decimal(8,6) nullable
RocAuc decimal(8,6) nullable
HammingLoss decimal(8,6) nullable
IsActive boolean not null default false
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Seed:

```text
ModelName = LightGBM
Version = Final_LightGBM_v1
MicroF1 = 0.9004
RocAuc = 0.9580
IsActive = true
```

---

### 5.13 AiPredictionRuns

Alanlar:

```text
Id int primary key
AssessmentId int not null foreign key -> Assessments.Id
ModelVersionId int not null foreign key -> ModelVersions.Id
RequestedByUserId int not null foreign key -> Users.Id
Status varchar(50) not null
ErrorMessage text nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Status değerleri:

```text
Pending
Success
Failed
```

---

### 5.14 AiPredictedActions

Alanlar:

```text
Id int primary key
PredictionRunId int not null foreign key -> AiPredictionRuns.Id
ActionCode varchar(50) not null
Probability decimal(8,6) not null
RankOrder int not null
IsSelected boolean not null default true
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Kural:

```text
ActionCode, ActionCatalogs.ActionId ile eşleşir.
Foreign key zorunlu olmayabilir; çünkü fallback desteklenmelidir.
```

---

### 5.15 ActionPlans

Alanlar:

```text
Id int primary key
AssessmentId int not null foreign key -> Assessments.Id
EmployeeId int not null foreign key -> Employees.Id
CreatedByUserId int not null foreign key -> Users.Id
Status varchar(50) not null
ApprovedAt timestamptz nullable
SentAt timestamptz nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Status değerleri:

```text
Draft
Edited
Approved
Sent
Completed
Cancelled
```

Duplicate rule:

```text
Aynı assessment için Draft, Edited, Approved veya Sent durumda ikinci action plan üretilmemelidir.
```

---

### 5.16 ActionPlanItems

Alanlar:

```text
Id int primary key
ActionPlanId int not null foreign key -> ActionPlans.Id
ActionCatalogId varchar(50) nullable foreign key -> ActionCatalogs.ActionId
AiPredictedActionId int nullable foreign key -> AiPredictedActions.Id
Title text not null
Description text not null
Resource text nullable
DeliveryType varchar(100) nullable
Priority varchar(50) not null
DueDate timestamptz nullable
Source varchar(50) not null
OrderNo int not null
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Source değerleri:

```text
AI
EditedAI
Manual
```

Kritik:

```text
ActionPlanItem için ayrı Status alanı oluşturulmayacaktır.
Silme işlemleri IsDeleted = true ile yapılacaktır.
Çalışana atandıktan sonraki görev durumu EmployeeTask.Status üzerinde takip edilir.
```

Snapshot kuralı:

```text
Title, Description, Resource, DeliveryType ActionCatalogs.ContentData içinden çıkarılıp ActionPlanItems içine snapshot olarak yazılmalıdır.
```

---

### 5.17 EmployeeTasks

Alanlar:

```text
Id int primary key
ActionPlanItemId int not null foreign key -> ActionPlanItems.Id
EmployeeId int not null foreign key -> Employees.Id
AssignedByUserId int not null foreign key -> Users.Id
Status varchar(50) not null
AssignedAt timestamptz not null
DueDate timestamptz nullable
CompletedAt timestamptz nullable
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

Status değerleri:

```text
Assigned
InProgress
Completed
Cancelled
```

Idempotency rule:

```text
Send endpoint iki kez çağrılırsa aynı ActionPlanItem için duplicate EmployeeTask oluşmamalıdır.
```

---

### 5.18 TaskComments

Opsiyonel.

Alanlar:

```text
Id int primary key
TaskId int not null foreign key -> EmployeeTasks.Id
UserId int not null foreign key -> Users.Id
CommentText text not null
CreatedAt timestamptz not null default now()
UpdatedAt timestamptz nullable
IsDeleted boolean not null default false
```

---

## 7. Foreign Key Relationship Summary

```text
Users.RoleId -> Roles.Id
Users.EmployeeId -> Employees.Id nullable
RefreshTokens.UserId -> Users.Id

Employees.DepartmentId -> Departments.Id
Employees.JobRoleId -> JobRoles.Id
Employees.ManagerId -> Employees.Id nullable

JobRoles.DepartmentId -> Departments.Id nullable

Assessments.EmployeeId -> Employees.Id
Assessments.CycleId -> AssessmentCycles.Id
Assessments.CreatedByUserId -> Users.Id

AssessmentScores.AssessmentId -> Assessments.Id
AssessmentScores.CompetencyId -> Competencies.Id

FeedbackComments.AssessmentId -> Assessments.Id

AiPredictionRuns.AssessmentId -> Assessments.Id
AiPredictionRuns.ModelVersionId -> ModelVersions.Id
AiPredictionRuns.RequestedByUserId -> Users.Id

AiPredictedActions.PredictionRunId -> AiPredictionRuns.Id

ActionPlanItems.ActionCatalogId -> ActionCatalogs.ActionId nullable
ActionPlanItems.AiPredictedActionId -> AiPredictedActions.Id nullable
ActionPlanItems.ActionPlanId -> ActionPlans.Id

ActionPlans.AssessmentId -> Assessments.Id
ActionPlans.EmployeeId -> Employees.Id
ActionPlans.CreatedByUserId -> Users.Id

EmployeeTasks.ActionPlanItemId -> ActionPlanItems.Id
EmployeeTasks.EmployeeId -> Employees.Id
EmployeeTasks.AssignedByUserId -> Users.Id

TaskComments.TaskId -> EmployeeTasks.Id
TaskComments.UserId -> Users.Id
```

Cascade delete rule:

```text
Kritik iş tablolarında cascade delete kullanılmamalıdır.
Restrict / NoAction tercih edilmelidir.
Soft delete uygulanmalıdır.
```

---

## 8. Enum / Status Storage Rule

Tüm status ve enum benzeri alanlar PostgreSQL'de string olarak saklanmalıdır.

Örnek:

```text
ActionPlan.Status = Draft
EmployeeTask.Status = Assigned
EvaluatorType = Manager
Priority = Medium
Source = AI
```

EF Core tarafında `HasConversion<string>()` kullanılmalıdır.

---

## 9. Seed Data Gereksinimleri

Agent en az aşağıdaki seed datayı oluşturmalıdır.

### 8.1 Roles

```text
Admin
HR
Manager
Employee
```

### 8.2 Demo Users

```text
admin@demo.com / Admin1234!
hr@demo.com / Hr1234!
manager@demo.com / Manager1234!
employee@demo.com / Employee1234!
```

Önemli:

```text
Bu şifreler mock repository'deki BCrypt hash'lerle birebir eşleşmelidir.
Mock'tan gerçek DB'ye geçişte login kırılmaması için şifreler aynı olmalıdır.
Agent önce mevcut backend kodundaki mock/demo credential değerlerini kontrol etmelidir.
Eğer mevcut backend farklı şifrelerle geliştirilmişse ya DB seed bu değerlere göre güncellenmeli ya da mock credentiallar bu dokümandaki nihai değerlerle eşitlenmelidir.
Final durumda mock ve gerçek DB demo credentialları aynı olmalıdır.
```

### 8.3 Demo Employees

En az iki Employee kaydı olmalıdır:

```text
Manager employee kaydı
Demo employee kaydı
```

Kural:

```text
manager@demo.com User.EmployeeId -> manager employee id
employee@demo.com User.EmployeeId -> demo employee id
Demo employee.ManagerId -> manager employee id
```

Demo employee tüm AI feature alanlarına sahip olmalıdır.

### 8.4 Departments + JobRoles

En az şu değerler seedlenmelidir:

```text
Departments:
Technology
Human Resources
Sales & Marketing
Finance & Accounting
Operations

JobRoles:
Software Engineer
Data Scientist
HR Specialist
Sales Executive
Operations Manager
Accountant
```

### 8.5 Competencies

13 feature competency seedlenmelidir:

```text
Core_Communication
Core_Teamwork
Core_ProblemSolving
Core_Adaptability
Core_Initiative
Core_Accountability
Core_LearningAgility
Core_TimeManagement
Dept_Comp1
Dept_Comp2
Dept_Comp3
Role_Comp1
Role_Comp2
```

### 8.6 Demo Assessment + Scores

Demo employee için en az bir completed assessment ve her competency için skor olmalıdır.

```text
Assessment.Status = Completed
Assessment.OverallScore = 3.5
AssessmentScores = 13 competency için skorlar
Basit demo için AssessmentScores.EvaluatorType = Manager kullanılabilir
```

### 8.7 ModelVersion

```text
ModelName = LightGBM
Version = Final_LightGBM_v1
MicroF1 = 0.9004
RocAuc = 0.9580
IsActive = true
```

### 8.8 ActionCatalogs

`db.sql` içindeki tüm `ActionCatalogs` seed verileri taşınmalıdır.

Önemli:

```text
Final DB seed aşamasında modelin üretebildiği tüm 47 action label için ActionCatalogs kaydı bulunmalıdır.
```

---

## 10. Migration / SQL Dosyası Beklentisi

Agent iki çıktı hazırlamalıdır:

```text
1. EF Core entity + configuration + migration dosyaları (zorunlu)
2. Full PostgreSQL schema SQL dosyası (zorunlu)
```

Full SQL dosyası adı:

```text
bitirme_full_database_schema.sql
```

Bu SQL dosyası:

- Tüm tabloları oluşturmalı.
- Foreign key ilişkilerini kurmalı.
- Unique constraintleri eklemeli.
- Indexleri eklemeli.
- Seed datayı içermeli.
- `ActionCatalogs` için verilen `db.sql` içeriğini kullanmalı.

Not:

```text
SQL dosyası migration sorun çıkarırsa fallback olarak kullanılacaktır.
Bu nedenle zorunludur.
```

---

## 11. Gerekli Index ve Constraintler

### Unique Constraintler

```text
Roles.Name
Users.Email
RefreshTokens.TokenHash
Departments.Code
Employees.EmployeeCode
Employees.Email
Competencies.Code
ActionCatalogs.ActionId
```

### Önerilen Indexler

```text
Users.RoleId
Users.EmployeeId
Employees.DepartmentId
Employees.JobRoleId
Employees.ManagerId
Assessments.EmployeeId
AssessmentScores.AssessmentId
AssessmentScores.CompetencyId
AiPredictionRuns.AssessmentId
AiPredictedActions.PredictionRunId
ActionPlans.AssessmentId
ActionPlans.EmployeeId
ActionPlanItems.ActionPlanId
ActionPlanItems.ActionCatalogId
EmployeeTasks.EmployeeId
EmployeeTasks.ActionPlanItemId
```

### Partial Unique Indexler

Duplicate action plan ve duplicate employee task oluşmaması sadece service logic'e bırakılmamalı, mümkün olduğunda PostgreSQL seviyesinde de desteklenmelidir.

Önerilen partial unique indexler:

```sql
CREATE UNIQUE INDEX "UX_EmployeeTasks_ActionPlanItemId_Active"
ON "EmployeeTasks" ("ActionPlanItemId")
WHERE "IsDeleted" = false;

CREATE UNIQUE INDEX "UX_ActionPlans_AssessmentId_ActivePlan"
ON "ActionPlans" ("AssessmentId")
WHERE "IsDeleted" = false
AND "Status" IN ('Draft', 'Edited', 'Approved', 'Sent');
```

Not:

```text
Bu indexler EF Core migration ile oluşturulmalıdır.
Provider desteği nedeniyle Fluent API yeterli olmazsa migrationBuilder.Sql(...) kullanılabilir.
```

### Check Constraintler

Önerilen check constraintler:

```sql
CHECK ("Score" >= 0 AND "Score" <= 5)
CHECK ("Probability" >= 0 AND "Probability" <= 1)
CHECK ("MinScore" <= "MaxScore")
CHECK ("Age" > 0)
CHECK ("EstimatedEffortHours" >= 0)
```

Uygulanacağı yerler:

```text
AssessmentScores.Score -> 0..5
AiPredictedActions.Probability -> 0..1
ActionCatalogs.MinScore <= ActionCatalogs.MaxScore
Employees.Age > 0
ActionCatalogs.EstimatedEffortHours >= 0
```

---

## 12. Backend Compatibility Rules

Agent veritabanını oluştururken şu backend kurallarını bozmamalıdır:

1. AI önerisi ham haliyle `AiPredictedActions` tablosunda saklanır.
2. Yetkili tarafından düzenlenen plan maddesi `ActionPlanItems` tablosunda saklanır.
3. Çalışana gönderilen görevler `EmployeeTasks` tablosunda saklanır.
4. `ActionPlanItems` içinde title/description/resource/delivery type snapshot olarak tutulur.
5. `ActionPlanItem` için ayrı status alanı yoktur; soft delete kullanılır.
6. `EmployeeTask.Status` görev takibini yönetir.
7. `RefreshTokens` raw token değil TokenHash tutar.
8. `ActionCatalogs.ActionId`, AI action code ile birebir eşleşir.
9. `EmployeeFeatureDto`, Employee + Assessment + AssessmentScore + Competency mapping ile üretilir.
10. `PerformanceScore` zorunlu feature alanıdır. Generate action plan akışında öncelik `Assessment.OverallScore` değerindedir; `Employee.PerformanceScore` son/genel skor cache alanı olarak tutulabilir.
11. `ActionCatalogs.ContentData` JSONB parsing kuralı:
    - Core competency aksiyonları için: `content["title"]`, `content["description"]`, `content["resource"]`, `content["delivery_type"]`
    - Department/Role bazlı aksiyonlar için nested JSON kullanılır:
      - Önce employee `JobRole` adıyla eşleşen key aranır.
      - Bulunamazsa employee `Department` adıyla eşleşen key aranır.
      - O da yoksa JSON içindeki ilk object değeri kullanılır.
    - Hiç parse edilemezse fallback uygulanır:
      `Title: "Gelişim Aksiyonu: {ActionCode}"`, `Description: "Bu aksiyon için lütfen yöneticinizle iletişime geçin."`, `Priority: Medium`
    - Bu parsing mantığı `ActionPlanService.GenerateDraftActionPlanAsync` içinde uygulanacaktır.

---

## 13. Acceptance Criteria

DB işi tamamlandı kabul edilmeden önce aşağıdakiler sağlanmalıdır:

```text
[ ] PostgreSQL database oluşturuldu
[ ] EF Core migration başarıyla çalışıyor
[ ] Tüm zorunlu tablolar var
[ ] Tüm foreign key ilişkileri var
[ ] Unique constraintler var
[ ] Partial unique indexler var: active ActionPlan per Assessment ve active EmployeeTask per ActionPlanItem
[ ] Check constraintler var: Score, Probability, MinScore/MaxScore, Age, EstimatedEffortHours
[ ] Enum/status alanları string tutuluyor
[ ] Soft delete alanları var
[ ] ActionCatalogs tablosu db.sql ile uyumlu
[ ] ActionCatalog entity string ActionId primary key kullanıyor; int Id kullanılmıyor
[ ] db.sql seed datası EF migration için geçerli INSERT formatına veya ayrı psql seed scriptine dönüştürüldü
[ ] ActionCatalogs içinde tüm AI action label kayıtları var
[ ] Demo users seedlendi
[ ] Demo manager + employee ilişkilendirildi
[ ] Demo employee için completed assessment var
[ ] Demo assessment için 13 competency score var
[ ] Demo assessment score kayıtlarında basit demo için EvaluatorType = Manager veya çoklu evaluator varsa ortalama kuralı uygulanıyor
[ ] ModelVersion seedlendi
[ ] ActionPlanItems.ActionCatalogId -> ActionCatalogs.ActionId çalışıyor
[ ] ActionPlanItem snapshot alanları var
[ ] EmployeeTasks duplicate oluşmasını engelleyecek yapı uygun
[ ] RefreshToken TokenHash unique
[ ] Backend `dotnet ef database update` sonrası çalışıyor
[ ] Swagger'dan login + employee features + generate action plan flow test edilebiliyor
```

---

## 14. Agent Prompt

Aşağıdaki prompt agentic AI aracına verilebilir:

```text
You are responsible for completing the database integration phase of an already implemented ASP.NET Core 9.0 backend.
The backend has already been developed based on backend_agentic_ai_task_list_v5.md.
Do NOT rewrite unrelated backend modules, controllers, services, or business logic.
Only update the DB integration layer, EF Core entities/configurations/AppDbContext/migrations/seed data, repository implementations if required, and ActionCatalog-related mapping required for db.sql compatibility.

Use backend_agentic_ai_task_list_v5.md as the main backend architecture reference.
Use db.sql only as the ActionCatalogs table and seed data reference.
Do not treat db.sql as the full backend schema.

Implement EF Core entities, configurations, AppDbContext, migrations, and seed data.
Also produce a full SQL schema file: bitirme_full_database_schema.sql
This file must include all tables, foreign keys, unique constraints, partial unique indexes, check constraints, indexes, and seed data.

CRITICAL: ActionCatalog entity must be rewritten with string ActionId as primary key.
The old/simplified ActionCatalog entity with int Id or Code/Title/Description columns must not be used as the DB source of truth.
ActionCatalogs must follow db.sql:
- ActionId varchar(50) primary key
- TargetCompetency varchar(50)
- ActionCategory varchar(50)
- ActionType varchar(50)
- Difficulty varchar(20)
- EstimatedEffortHours integer
- MinScore numeric(3,2)
- MaxScore numeric(3,2)
- ContentData jsonb
- plus IsActive, CreatedAt, UpdatedAt, IsDeleted

MockActionCatalogRepository must also be updated to use string key lookups.
ActionPlanItem.ActionCatalogId must be string (varchar 50) referencing ActionCatalogs.ActionId.
IActionCatalogRepository.GetByCodesAsync(IEnumerable<string> codes) signature must be preserved.

db.sql may contain pg_dump/psql constructs such as COPY FROM stdin, \restrict, and \.
Do not paste raw db.sql into migrationBuilder.Sql(...).
Convert ActionCatalogs seed data into valid INSERT INTO statements for EF migration, or provide a separate psql seed script.

ActionPlanItems must store Title, Description, Resource, DeliveryType as snapshot fields parsed from ActionCatalogs.ContentData at plan generation time:
- Core actions: parse content["title"], content["description"], content["resource"], content["delivery_type"]
- Department/Role based actions: try employee JobRole key first, then Department key, then first object
- If parsing fails: use fallback title/description
ContentData may be stored as string with jsonb column type, but parsing must use System.Text.Json.JsonDocument or equivalent safe JSON parser.

Create all required tables:
Roles, Users, RefreshTokens, Departments, JobRoles, Employees, AssessmentCycles, Assessments,
Competencies, AssessmentScores, FeedbackComments, ModelVersions, AiPredictionRuns,
AiPredictedActions, ActionCatalogs, ActionPlans, ActionPlanItems, EmployeeTasks, TaskComments.

Store enum/status values as strings using HasConversion<string>().
Use soft delete fields (IsDeleted).
Avoid cascade delete for critical business tables — use Restrict/NoAction.

Add constraints and indexes:
- Unique constraints: Roles.Name, Users.Email, RefreshTokens.TokenHash, Departments.Code, Employees.EmployeeCode, Employees.Email, Competencies.Code, ActionCatalogs.ActionId
- Partial unique index: active EmployeeTasks by ActionPlanItemId
- Partial unique index: active ActionPlans by AssessmentId when Status in Draft/Edited/Approved/Sent
- Check constraints: AssessmentScores.Score 0..5, AiPredictedActions.Probability 0..1, ActionCatalogs.MinScore <= MaxScore, Employees.Age > 0, EstimatedEffortHours >= 0

PerformanceScore rule:
Generate action plan flow should use Assessments.OverallScore as the primary PerformanceScore sent to ML.
Employees.PerformanceScore may remain as a latest/general score cache.
Seed both values consistently for demo data.

Seed data requirements:
- Roles: Admin, HR, Manager, Employee
- Demo users with BCrypt hashed passwords. First check existing mock/demo credentials in the implemented backend. Final DB credentials and mock credentials must match. Preferred values: admin@demo.com/Admin1234!, hr@demo.com/Hr1234!, manager@demo.com/Manager1234!, employee@demo.com/Employee1234!
- AssessmentCycle: "2025 Yılı Q4 Değerlendirmesi", 2025-10-01 to 2025-12-31, Status=Completed
- Manager employee record linked to manager@demo.com
- Demo employee record linked to employee@demo.com, ManagerId pointing to manager employee
- Demo employee must have all AI feature fields populated
- Completed assessment for demo employee with 13 competency scores
- For simple demo seed, use EvaluatorType=Manager for each competency score. If multiple evaluator scores are seeded, backend feature generation must average scores by Competency.Code.
- ModelVersion: LightGBM Final_LightGBM_v1, MicroF1=0.9004, RocAuc=0.9580, IsActive=true
- All ActionCatalogs data from db.sql

Backend compatibility rules:
- AiPredictedAction stores raw AI recommendations.
- ActionPlanItem stores edited/final plan item snapshot.
- EmployeeTask stores assigned task and status.
- ActionPlanItem has no separate Status field; deletion uses IsDeleted.
- EmployeeTask.Status manages task lifecycle.
- RefreshTokens store TokenHash, not raw token.
- EmployeeFeatureDto is generated from Employee + Assessment + AssessmentScore/Competency mapping; do not add all competency scores as Employee columns.

After implementation, run:
- dotnet build
- dotnet ef database update
- dotnet test if tests exist
Fix all errors.
```
