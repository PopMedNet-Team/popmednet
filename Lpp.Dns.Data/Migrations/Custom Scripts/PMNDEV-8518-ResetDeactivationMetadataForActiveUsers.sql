BEGIN TRANSACTION

SELECT * FROM Users u WHERE u.isActive = 1 AND u.isDeleted = 0 AND (u.DeactivatedOn IS NOT NULL OR u.DeactivatedByID IS NOT NULL OR ISNULL(u.DeactivationReason, '') <> '')

UPDATE Users SET DeactivatedByID = NULL, DeactivatedOn = NULL, DeactivationReason = NULL WHERE isActive = 1 AND isDeleted = 0 AND (DeactivatedOn IS NOT NULL OR DeactivatedByID IS NOT NULL OR ISNULL(DeactivationReason, '') <> '')

COMMIT TRANSACTION