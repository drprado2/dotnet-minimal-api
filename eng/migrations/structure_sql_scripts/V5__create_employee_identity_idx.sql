USE [MyCompany]
GO

CREATE UNIQUE INDEX idx_uk_identity_user_id ON Employee (IdentityUserId);
