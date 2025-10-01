-- Script de inicialização do banco de dados
CREATE DATABASE IF NOT EXISTS usersdb;
USE usersdb;

-- Criar usuário para a API se não existir
CREATE USER IF NOT EXISTS 'apiuser'@'%' IDENTIFIED BY 'apipassword';
GRANT ALL PRIVILEGES ON usersdb.* TO 'apiuser'@'%';
FLUSH PRIVILEGES;

-- =====================================================
-- CRIAÇÃO DAS TABELAS
-- =====================================================

-- Tabela de Usuários
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX IX_Users_Email (Email)
);

-- Tabela de Níveis de Acesso
CREATE TABLE IF NOT EXISTS AccessLevels (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(200) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX IX_AccessLevels_Name (Name)
);

-- Tabela de Relacionamento Usuário x Nível de Acesso
CREATE TABLE IF NOT EXISTS UserAccessLevels (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    AccessLevelId INT NOT NULL,
    AssignedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    RevokedAt DATETIME NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- Foreign Keys
    CONSTRAINT FK_UserAccessLevels_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    
    CONSTRAINT FK_UserAccessLevels_AccessLevels 
        FOREIGN KEY (AccessLevelId) REFERENCES AccessLevels(Id) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    
    -- Indexes
    INDEX IX_UserAccessLevels_UserId (UserId),
    INDEX IX_UserAccessLevels_AccessLevelId (AccessLevelId),
    INDEX IX_UserAccessLevels_User_Access_Active (UserId, AccessLevelId, IsActive),
    
    -- Constraint para evitar duplicação de acesso ativo
    UNIQUE KEY UK_UserAccessLevels_Active (UserId, AccessLevelId, IsActive)
);




-- =====================================================
-- INSERÇÃO DE DADOS INICIAIS
-- =====================================================

-- Inserir níveis de acesso padrão
INSERT INTO AccessLevels (Name, Description, CreatedAt, UpdatedAt) VALUES
('Administrator', 'Acesso total ao sistema com todas as permissões', NOW(), NOW()),
('Manager', 'Acesso de gerenciamento com permissões administrativas limitadas', NOW(), NOW()),
('User', 'Acesso básico de usuário final com permissões limitadas', NOW(), NOW()),
('Viewer', 'Acesso apenas de visualização, sem permissões de edição', NOW(), NOW())
ON DUPLICATE KEY UPDATE 
    Description = VALUES(Description),
    UpdatedAt = NOW();

-- Inserir usuário administrador padrão (senha: admin123)
-- Hash BCrypt para 'admin123': $2a$12$1nbWG5nXfpUE/kB5w7I2cehTskJ4FLNYHyAwglyeZrTDkbKWMX3M2
INSERT INTO Users (Name, Email, PasswordHash, CreatedAt, UpdatedAt) VALUES
('Administrador', 'admin@sistema.com', '$2a$12$1nbWG5nXfpUE/kB5w7I2cehTskJ4FLNYHyAwglyeZrTDkbKWMX3M2', NOW(), NOW())
ON DUPLICATE KEY UPDATE 
    Name = VALUES(Name),
    UpdatedAt = NOW();

-- Atribuir nível Administrator ao usuário padrão
INSERT INTO UserAccessLevels (UserId, AccessLevelId, AssignedAt, IsActive)
SELECT u.Id, a.Id, NOW(), TRUE
FROM Users u, AccessLevels a
WHERE u.Email = 'admin@sistema.com' 
  AND a.Name = 'Administrator'
ON DUPLICATE KEY UPDATE 
    IsActive = TRUE,
    AssignedAt = NOW(),
    RevokedAt = NULL;



-- =====================================================
-- VERIFICAÇÕES E RELATÓRIOS
-- =====================================================

-- Mostrar estrutura das tabelas criadas
SHOW TABLES;

-- Mostrar dados iniciais
SELECT 'USUARIOS CADASTRADOS:' as Info;
SELECT Id, Name, Email, CreatedAt FROM Users;

SELECT 'NIVEIS DE ACESSO DISPONÍVEIS:' as Info;
SELECT Id, Name, Description FROM AccessLevels;

SELECT 'ASSOCIAÇÕES ATIVAS:' as Info;
SELECT 
    u.Name as Usuario,
    al.Name as NivelAcesso,
    ual.AssignedAt as AtribuidoEm
FROM UserAccessLevels ual
JOIN Users u ON ual.UserId = u.Id
JOIN AccessLevels al ON ual.AccessLevelId = al.Id
WHERE ual.IsActive = TRUE;

SELECT 'SETUP COMPLETO!' as Status;