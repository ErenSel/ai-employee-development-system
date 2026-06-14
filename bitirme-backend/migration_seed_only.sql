-- =============================================================================
-- Full seed data — INSERT only, safe to run on a completely empty database
-- (after schema migrations have been applied via dotnet ef database update)
-- All statements use ON CONFLICT DO NOTHING so the file is idempotent.
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1. Roles
-- -----------------------------------------------------------------------------
INSERT INTO "Roles" ("Id", "CreatedAt", "Description", "IsDeleted", "Name", "UpdatedAt")
VALUES
    (1, '2025-01-01 00:00:00Z', 'Full system access',         false, 'Admin',    NULL),
    (2, '2025-01-01 00:00:00Z', 'Human resources management', false, 'HR',       NULL),
    (3, '2025-01-01 00:00:00Z', 'Team management',            false, 'Manager',  NULL),
    (4, '2025-01-01 00:00:00Z', 'Standard employee access',   false, 'Employee', NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 2. Departments
-- -----------------------------------------------------------------------------
INSERT INTO "Departments" ("Id", "Code", "CreatedAt", "IsDeleted", "Name", "UpdatedAt")
VALUES
    (1, 'HR',    '2025-01-01 00:00:00Z', false, 'Human Resources',      NULL),
    (2, 'TECH',  '2025-01-01 00:00:00Z', false, 'Technology',           NULL),
    (3, 'SALES', '2025-01-01 00:00:00Z', false, 'Sales & Marketing',    NULL),
    (4, 'FIN',   '2025-01-01 00:00:00Z', false, 'Finance & Accounting', NULL),
    (5, 'OPS',   '2025-01-01 00:00:00Z', false, 'Operations',           NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 3. JobRoles
-- -----------------------------------------------------------------------------
INSERT INTO "JobRoles" ("Id", "CreatedAt", "DepartmentId", "Description", "IsDeleted", "Name", "UpdatedAt")
VALUES
    -- HR (DepartmentId=1)
    (1,  '2025-01-01 00:00:00Z', 1, NULL, false, 'HR Specialist',            NULL),
    (2,  '2025-01-01 00:00:00Z', 1, NULL, false, 'Recruiter',                NULL),
    (3,  '2025-01-01 00:00:00Z', 1, NULL, false, 'HR Manager',               NULL),
    -- Technology (DepartmentId=2)
    (4,  '2025-01-01 00:00:00Z', 2, NULL, false, 'Software Engineer',        NULL),
    (5,  '2025-01-01 00:00:00Z', 2, NULL, false, 'Senior Software Engineer', NULL),
    (6,  '2025-01-01 00:00:00Z', 2, NULL, false, 'Data Scientist',           NULL),
    (11, '2025-01-01 00:00:00Z', 2, NULL, false, 'Engineering Manager',      NULL),
    -- Sales & Marketing (DepartmentId=3)
    (12, '2025-01-01 00:00:00Z', 3, NULL, false, 'Sales Executive',          NULL),
    (13, '2025-01-01 00:00:00Z', 3, NULL, false, 'Sales Representative',     NULL),
    (14, '2025-01-01 00:00:00Z', 3, NULL, false, 'Account Manager',          NULL),
    (15, '2025-01-01 00:00:00Z', 3, NULL, false, 'Marketing Specialist',     NULL),
    -- Finance & Accounting (DepartmentId=4)
    (16, '2025-01-01 00:00:00Z', 4, NULL, false, 'Accountant',               NULL),
    (17, '2025-01-01 00:00:00Z', 4, NULL, false, 'Financial Analyst',        NULL),
    (18, '2025-01-01 00:00:00Z', 4, NULL, false, 'Payroll Specialist',       NULL),
    (19, '2025-01-01 00:00:00Z', 4, NULL, false, 'Finance Manager',          NULL),
    -- Operations (DepartmentId=5)
    (20, '2025-01-01 00:00:00Z', 5, NULL, false, 'Operations Specialist',    NULL),
    (21, '2025-01-01 00:00:00Z', 5, NULL, false, 'Logistics Coordinator',    NULL),
    (22, '2025-01-01 00:00:00Z', 5, NULL, false, 'Production Engineer',      NULL),
    (24, '2025-01-01 00:00:00Z', 5, NULL, false, 'Operations Manager',       NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 4. Competencies (13 items)
-- -----------------------------------------------------------------------------
INSERT INTO "Competencies" ("Id", "Category", "Code", "CreatedAt", "IsActive", "IsDeleted", "Name", "UpdatedAt")
VALUES
    (1,  'Core',       'Core_Communication',   '2025-01-01 00:00:00Z', true, false, 'İletişim',              NULL),
    (2,  'Core',       'Core_Teamwork',        '2025-01-01 00:00:00Z', true, false, 'Takım Çalışması',       NULL),
    (3,  'Core',       'Core_ProblemSolving',  '2025-01-01 00:00:00Z', true, false, 'Problem Çözme',         NULL),
    (4,  'Core',       'Core_Adaptability',    '2025-01-01 00:00:00Z', true, false, 'Uyum Yeteneği',         NULL),
    (5,  'Core',       'Core_Initiative',      '2025-01-01 00:00:00Z', true, false, 'İnisiyatif Alma',       NULL),
    (6,  'Core',       'Core_Accountability',  '2025-01-01 00:00:00Z', true, false, 'Sorumluluk Bilinci',    NULL),
    (7,  'Core',       'Core_LearningAgility', '2025-01-01 00:00:00Z', true, false, 'Öğrenme Çevikliği',    NULL),
    (8,  'Core',       'Core_TimeManagement',  '2025-01-01 00:00:00Z', true, false, 'Zaman Yönetimi',        NULL),
    (9,  'Department', 'Dept_Comp1',           '2025-01-01 00:00:00Z', true, false, 'Departman Yetkinliği 1', NULL),
    (10, 'Department', 'Dept_Comp2',           '2025-01-01 00:00:00Z', true, false, 'Departman Yetkinliği 2', NULL),
    (11, 'Department', 'Dept_Comp3',           '2025-01-01 00:00:00Z', true, false, 'Departman Yetkinliği 3', NULL),
    (12, 'Role',       'Role_Comp1',           '2025-01-01 00:00:00Z', true, false, 'Rol Yetkinliği 1',      NULL),
    (13, 'Role',       'Role_Comp2',           '2025-01-01 00:00:00Z', true, false, 'Rol Yetkinliği 2',      NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 5. ModelVersion
-- -----------------------------------------------------------------------------
INSERT INTO "ModelVersions" ("Id", "CreatedAt", "Description", "IsActive", "IsDeleted", "MicroF1", "ModelName", "RocAuc", "UpdatedAt", "Version")
VALUES
    (1, '2025-01-01 00:00:00Z',
     'LightGBM multi-label classifier for action recommendation',
     true, false, 0.9004, 'LightGBM', 0.9580, NULL, 'Final_LightGBM_v1')
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 6. AssessmentCycle
-- -----------------------------------------------------------------------------
INSERT INTO "AssessmentCycles" ("Id", "CreatedAt", "EndDate", "IsDeleted", "Name", "StartDate", "Status", "UpdatedAt")
VALUES
    (1, '2025-01-01 00:00:00Z', '2025-12-31 00:00:00Z', false,
     '2025 Yılı Q4 Değerlendirmesi', '2025-10-01 00:00:00Z', 'Completed', NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 7. Employees — managers (ManagerId IS NULL) inserted first to satisfy FK
-- -----------------------------------------------------------------------------
INSERT INTO "Employees" ("Id", "Age", "Attrition", "BusinessTravel", "CreatedAt", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "Gender", "IsActive", "IsDeleted", "JobRoleId", "JobSatisfaction", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "UpdatedAt", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager")
VALUES
    -- ── Managers (ManagerId = NULL) ──────────────────────────────────────────
    -- Ahmet Yılmaz — HR Manager
    (1,  42, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 1,  8, '4', 'Human Resources',       'ahmet.yilmaz@demo.com', 'EMP001', 4, 'Ahmet Yılmaz',  'Male',   true, false,  3, 4, NULL, 'Married',  4.2, 18, NULL, 3, 8,  4, 0),
    -- Zeynep Arslan — Engineering Manager
    (5,  39, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 2, 15, '4', 'Computer Science',      'zeynep.arslan@demo.com','EMP005', 4, 'Zeynep Arslan', 'Female', true, false, 11, 4, NULL, 'Married',  4.5, 16, NULL, 3, 6,  3, 0),
    -- Hakan Çelik — Sales Executive
    (9,  45, 'No', 'Travel_Frequently', '2025-01-01 00:00:00Z', 3, 20, '3', 'Marketing',             'hakan.celik@demo.com',  'EMP009', 3, 'Hakan Çelik',   'Male',   true, false, 12, 3, NULL, 'Married',  4.0, 22, NULL, 2, 12, 6, 0),
    -- Kemal Şahin — Finance Manager
    (13, 48, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 4,  9, '4', 'Finance',               'kemal.sahin@demo.com',  'EMP013', 4, 'Kemal Şahin',   'Male',   true, false, 19, 4, NULL, 'Married',  4.4, 24, NULL, 3, 10, 5, 0),
    -- Orhan Yalçın — Operations Manager
    (17, 44, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 5, 10, '4', 'Industrial Engineering','orhan.yalcin@demo.com', 'EMP017', 3, 'Orhan Yalçın',  'Male',   true, false, 24, 4, NULL, 'Married',  4.3, 20, NULL, 3, 8,  4, 0)
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Employees" ("Id", "Age", "Attrition", "BusinessTravel", "CreatedAt", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "Gender", "IsActive", "IsDeleted", "JobRoleId", "JobSatisfaction", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "UpdatedAt", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager")
VALUES
    -- ── HR team — report to Ahmet (1) ────────────────────────────────────────
    (2,  27, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 1,  3, '3', 'Psychology',            'buse.demir@demo.com',   'EMP002', 3, 'Buse Demir',    'Female', true, false,  1, 4,  1, 'Single',   3.8,  5, NULL, 4, 3,  2, 2),
    (3,  31, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 1, 12, '3', 'Business Administration','cem.aydin@demo.com',    'EMP003', 3, 'Cem Aydın',     'Male',   true, false,  2, 3,  1, 'Married',  3.9,  8, NULL, 3, 4,  3, 3),
    (4,  29, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 1,  6, '3', 'Sociology',             'deniz.yildiz@demo.com', 'EMP004', 2, 'Deniz Yıldız',  'Female', true, false,  1, 3,  1, 'Single',   3.5,  6, NULL, 3, 2,  2, 2),
    -- ── Tech team — report to Zeynep (5) ─────────────────────────────────────
    (6,  26, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 2,  4, '3', 'Computer Science',      'emre.koc@demo.com',     'EMP006', 3, 'Emre Koç',      'Male',   true, false,  4, 3,  5, 'Single',   3.6,  4, NULL, 2, 2,  2, 2),
    (7,  34, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 2, 10, '3', 'Information Technology','elif.sen@demo.com',      'EMP007', 4, 'Elif Şen',      'Female', true, false,  5, 4,  5, 'Married',  4.1, 11, NULL, 4, 5,  2, 3),
    (8,  30, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 2,  5, '4', 'Mathematics',           'fatih.kaya@demo.com',   'EMP008', 3, 'Fatih Kaya',    'Male',   true, false,  6, 4,  5, 'Single',   3.9,  7, NULL, 3, 3,  2, 2),
    -- ── Sales team — report to Hakan Çelik (9) ───────────────────────────────
    (10, 28, 'No', 'Travel_Frequently', '2025-01-01 00:00:00Z', 3,  7, '3', 'Communication Studies', 'gizem.ozturk@demo.com', 'EMP010', 4, 'Gizem Öztürk',  'Female', true, false, 13, 3,  9, 'Single',   3.7,  6, NULL, 3, 3,  3, 3),
    (11, 35, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 3, 11, '3', 'Business',              'hakan.yilmaz@demo.com', 'EMP011', 3, 'Hakan Yılmaz',  'Male',   true, false, 14, 4,  9, 'Married',  3.9, 12, NULL, 3, 5,  3, 3),
    (12, 32, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 3,  4, '4', 'Marketing',             'irem.aslan@demo.com',   'EMP012', 3, 'İrem Aslan',    'Female', true, false, 15, 4,  9, 'Single',   4.1,  9, NULL, 4, 4,  2, 2),
    -- ── Finance team — report to Kemal Şahin (13) ────────────────────────────
    (14, 33, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 4,  5, '3', 'Accounting',            'lale.bulut@demo.com',   'EMP014', 3, 'Lale Bulut',    'Female', true, false, 16, 3, 13, 'Married',  3.8, 10, NULL, 4, 6,  4, 4),
    (15, 29, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 4,  8, '3', 'Economics',             'murat.guler@demo.com',  'EMP015', 3, 'Murat Güler',   'Male',   true, false, 17, 4, 13, 'Single',   4.0,  6, NULL, 3, 3,  2, 2),
    (16, 36, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 4, 13, '3', 'Business Administration','nalan.demir@demo.com',  'EMP016', 4, 'Nalan Demir',   'Female', true, false, 18, 3, 13, 'Married',  3.7, 13, NULL, 3, 7,  5, 5),
    -- ── Operations team — report to Orhan Yalçın (17) ────────────────────────
    (18, 28, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 5,  3, '3', 'Business',              'pinar.tekin@demo.com',  'EMP018', 3, 'Pınar Tekin',   'Female', true, false, 20, 3, 17, 'Single',   3.6,  5, NULL, 4, 3,  2, 2),
    (19, 37, 'No', 'Travel_Rarely',     '2025-01-01 00:00:00Z', 5, 18, '3', 'Logistics',             'riza.mutlu@demo.com',   'EMP019', 3, 'Rıza Mutlu',    'Male',   true, false, 21, 3, 17, 'Married',  3.9, 14, NULL, 3, 5,  3, 3),
    (20, 31, 'No', 'Non-Travel',        '2025-01-01 00:00:00Z', 5,  5, '3', 'Mechanical Engineering','selin.yilmaz@demo.com', 'EMP020', 4, 'Selin Yılmaz',  'Female', true, false, 22, 4, 17, 'Married',  4.0,  8, NULL, 3, 4,  3, 3)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 8. Users (22 total: 1 admin, 1 HR, 4 managers, 16 employees)
-- Passwords:  admin@demo.com = Admin1234!
--             hr@demo.com    = Hr1234!
--             everyone else  = Demo1234!
-- -----------------------------------------------------------------------------
INSERT INTO "Users" ("Id", "CreatedAt", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "PasswordHash", "RoleId", "UpdatedAt")
VALUES
    -- System accounts
    (1,  '2025-01-01 00:00:00Z', 'admin@demo.com',           NULL, 'Admin User',    true, false, '$2a$11$zfRkFzsfjJ/dMsTdAhq.ie1gKjkUH6hRdzo.8yAZTQLLWgpwhQ4QC', 1, NULL),
    (2,  '2025-01-01 00:00:00Z', 'hr@demo.com',              NULL, 'HR User',       true, false, '$2a$11$nCoV4NjJ9bj4oP5K3riM2e4gNOPO0Kk3rKmwWZI2ne6gXrnPM.Rym', 2, NULL),
    -- Managers (RoleId=3)
    (3,  '2025-01-01 00:00:00Z', 'ahmet.yilmaz@demo.com',   1,  'Ahmet Yılmaz',  true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 3, NULL),
    (7,  '2025-01-01 00:00:00Z', 'zeynep.arslan@demo.com',  5,  'Zeynep Arslan', true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 3, NULL),
    (11, '2025-01-01 00:00:00Z', 'hakan.celik@demo.com',    9,  'Hakan Çelik',   true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 3, NULL),
    (15, '2025-01-01 00:00:00Z', 'kemal.sahin@demo.com',    13, 'Kemal Şahin',   true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 3, NULL),
    (19, '2025-01-01 00:00:00Z', 'orhan.yalcin@demo.com',   17, 'Orhan Yalçın',  true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 3, NULL),
    -- Employees (RoleId=4)
    (4,  '2025-01-01 00:00:00Z', 'buse.demir@demo.com',     2,  'Buse Demir',    true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (5,  '2025-01-01 00:00:00Z', 'cem.aydin@demo.com',      3,  'Cem Aydın',     true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (6,  '2025-01-01 00:00:00Z', 'deniz.yildiz@demo.com',   4,  'Deniz Yıldız',  true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (8,  '2025-01-01 00:00:00Z', 'emre.koc@demo.com',       6,  'Emre Koç',      true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (9,  '2025-01-01 00:00:00Z', 'elif.sen@demo.com',        7,  'Elif Şen',      true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (10, '2025-01-01 00:00:00Z', 'fatih.kaya@demo.com',     8,  'Fatih Kaya',    true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (12, '2025-01-01 00:00:00Z', 'gizem.ozturk@demo.com',   10, 'Gizem Öztürk',  true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (13, '2025-01-01 00:00:00Z', 'hakan.yilmaz@demo.com',   11, 'Hakan Yılmaz',  true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (14, '2025-01-01 00:00:00Z', 'irem.aslan@demo.com',     12, 'İrem Aslan',    true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (16, '2025-01-01 00:00:00Z', 'lale.bulut@demo.com',     14, 'Lale Bulut',    true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (17, '2025-01-01 00:00:00Z', 'murat.guler@demo.com',    15, 'Murat Güler',   true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (18, '2025-01-01 00:00:00Z', 'nalan.demir@demo.com',    16, 'Nalan Demir',   true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (20, '2025-01-01 00:00:00Z', 'pinar.tekin@demo.com',    18, 'Pınar Tekin',   true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (21, '2025-01-01 00:00:00Z', 'riza.mutlu@demo.com',     19, 'Rıza Mutlu',    true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL),
    (22, '2025-01-01 00:00:00Z', 'selin.yilmaz@demo.com',   20, 'Selin Yılmaz',  true, false, '$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC', 4, NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 9. Demo Assessment — Buse Demir (EmployeeId=2), Q4 2025, Completed
-- -----------------------------------------------------------------------------
INSERT INTO "Assessments" ("Id", "CreatedAt", "CreatedByUserId", "CycleId", "EmployeeId", "IsDeleted", "OverallScore", "Status", "UpdatedAt")
VALUES
    (1, '2025-01-01 00:00:00Z', 2, 1, 2, false, 3.8, 'Completed', NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 10. Assessment Assignments — Self (Buse=2) + Manager (Ahmet=1)
-- -----------------------------------------------------------------------------
INSERT INTO "AssessmentAssignments" ("Id", "AssessmentId", "CompletedAt", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsCompleted", "IsDeleted", "UpdatedAt")
VALUES
    (1, 1, '2025-01-01 00:00:00Z', '2025-01-01 00:00:00Z', 2, 'Self',    true, false, NULL),
    (2, 1, '2025-01-01 00:00:00Z', '2025-01-01 00:00:00Z', 1, 'Manager', true, false, NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 11. Assessment Scores
--     Self scores    — EvaluatorEmployeeId=2 (Buse Demir),  EvaluatorType='Self'
--     Manager scores — EvaluatorEmployeeId=1 (Ahmet Yılmaz), EvaluatorType='Manager'
-- -----------------------------------------------------------------------------
INSERT INTO "AssessmentScores" ("Id", "AssessmentId", "CompetencyId", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsDeleted", "Score", "UpdatedAt")
VALUES
    -- Self (Buse Demir)
    (1,  1,  1, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.5, NULL),
    (2,  1,  2, '2025-01-01 00:00:00Z', 2, 'Self', false, 4.0, NULL),
    (3,  1,  3, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.8, NULL),
    (4,  1,  4, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.2, NULL),
    (5,  1,  5, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.0, NULL),
    (6,  1,  6, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.9, NULL),
    (7,  1,  7, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.6, NULL),
    (8,  1,  8, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.1, NULL),
    (9,  1,  9, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.7, NULL),
    (10, 1, 10, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.3, NULL),
    (11, 1, 11, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.8, NULL),
    (12, 1, 12, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.5, NULL),
    (13, 1, 13, '2025-01-01 00:00:00Z', 2, 'Self', false, 3.4, NULL),
    -- Manager (Ahmet Yılmaz)
    (14, 1,  1, '2025-01-01 00:00:00Z', 1, 'Manager', false, 3.8, NULL),
    (15, 1,  2, '2025-01-01 00:00:00Z', 1, 'Manager', false, 4.2, NULL),
    (16, 1,  3, '2025-01-01 00:00:00Z', 1, 'Manager', false, 4.0, NULL),
    (17, 1,  4, '2025-01-01 00:00:00Z', 1, 'Manager', false, 3.5, NULL),
    (18, 1,  5, '2025-01-01 00:00:00Z', 1, 'Manager', false, 3.6, NULL),
    (19, 1,  6, '2025-01-01 00:00:00Z', 1, 'Manager', false, 4.1, NULL),
    (20, 1,  7, '2025-01-01 00:00:00Z', 1, 'Manager', false, 3.9, NULL),
    (21, 1,  8, '2025-01-01 00:00:00Z', 1, 'Manager', false, 3.7, NULL),
    (22, 1,  9, '2025-01-01 00:00:00Z', 1, 'Manager', false, 4.0, NULL),
    (23, 1, 10, '2025-01-01 00:00:00Z', 1, 'Manager', false, 3.5, NULL),
    (24, 1, 11, '2025-01-01 00:00:00Z', 1, 'Manager', false, 4.0, NULL),
    (25, 1, 12, '2025-01-01 00:00:00Z', 1, 'Manager', false, 4.5, NULL),
    (26, 1, 13, '2025-01-01 00:00:00Z', 1, 'Manager', false, 3.8, NULL)
ON CONFLICT ("Id") DO NOTHING;

-- -----------------------------------------------------------------------------
-- 12. EF Migrations history — mark all 4 migrations as applied
-- -----------------------------------------------------------------------------
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES
    ('20260604173003_InitialCreate',          '10.0.0'),
    ('20260606231333_Add360EvaluatorSupport', '10.0.0'),
    ('20260606232557_AddZeynepUser',          '10.0.0'),
    ('20260611181247_UpdateDemoSeedData',     '10.0.0')
ON CONFLICT DO NOTHING;
