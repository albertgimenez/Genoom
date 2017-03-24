CREATE TABLE Person
(
    Id uniqueidentifier DEFAULT NEWID(),
    Name varchar(100) NOT NULL,
    LastName varchar(100),
    Birthdate datetime,
    Sex tinyint NOT NULL,
    PhotoFileName varchar(255),

    PRIMARY KEY (Id)
);

CREATE TABLE PersonFamily
(
    PersonId uniqueidentifier,
    RelatedPersonId uniqueidentifier,
    Relationship tinyint NOT NULL,

    PRIMARY KEY (PersonId, RelatedPersonId),
    FOREIGN KEY (PersonId) REFERENCES Person(Id),
    FOREIGN KEY (RelatedPersonId) REFERENCES Person(Id)
);

GO

CREATE VIEW PersonRelationshipView AS
(
    SELECT F.PersonId, P1.Name, P1.LastName, F.RelatedPersonId, P2.Name RelatedName, P2.LastName RelatedLastName, P2.Birthdate, P2.Sex, P2.PhotoFileName, F.RelationShip
    FROM PersonFamily F
    LEFT JOIN Person P1 ON F.PersonId = P1.Id
    LEFT JOIN Person P2 ON F.RelatedPersonId = P2.Id
);

GO

CREATE PROCEDURE [dbo].[AddChild]
    @ParentId uniqueidentifier,
    @Name varchar(100),
    @LastName varchar(100),
    @Birthdate datetime,
    @Sex tinyint,
    @PhotoFileName varchar(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @childId uniqueidentifier = NEWID();

        -- Add the new child
        INSERT INTO Person (Id, Name, LastName, Birthdate, Sex, PhotoFileName)
        OUTPUT inserted.Id
        VALUES
        (
            @childId,
            @Name,
            @Lastname,
            @Birthdate,
            @Sex,
            @PhotoFileName
        );

        -- Create relationship between parent-child
        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip) VALUES
        (
            @ParentId,
            @childId,
            3 --Child
        );

        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip) VALUES
        (
            @childId,
            @ParentId,
            1 --Parent
        );

        -- Create relationship between parent-child (the other parent)
        DECLARE @partnerId uniqueidentifier = (SELECT TOP 1 RelatedPersonId FROM PersonFamily WHERE PersonId = @ParentId AND Relationship = 0);
        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip) VALUES
        (
            @partnerId,
            @childId,
            3 --Child
        );

        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip) VALUES
        (
            @childId,
            @partnerId,
            1 --Parent
        );

        -- Create relationship between child and the other brothers (we look for the children of the parent, without the new one)
        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip)
            SELECT @childId, RelatedPersonId, 2 FROM PersonFamily WHERE PersonId = @ParentId AND RelatedPersonId <> @childId AND Relationship = 3;

        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip)
            SELECT RelatedPersonId, @childId, 2 FROM PersonFamily WHERE PersonId = @ParentId AND RelatedPersonId <> @childId AND Relationship = 3;

        COMMIT;
    END TRY

    BEGIN CATCH
        ROLLBACK;
    END CATCH;
END;

GO