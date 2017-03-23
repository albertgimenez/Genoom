CREATE TABLE Person
(
    Id uniqueidentifier DEFAULT NEWID(),
    Name varchar(100) NOT NULL,
    Lastname varchar(100),
    Birthdate datetime,
    Sex tinyint NOT NULL,
    PhotoFileName varchar(255),

    PRIMARY KEY (Id)
);

CREATE TABLE PersonFamily
(
    PersonId uniqueidentifier,
    RelatedPersonId uniqueidentifier,
    RelationShip tinyint NOT NULL,

    PRIMARY KEY (PersonId, RelatedPersonId),
    FOREIGN KEY (PersonId) REFERENCES Person(Id),
    FOREIGN KEY (RelatedPersonId) REFERENCES Person(Id)
);

GO

CREATE VIEW PersonRelationship AS
(
    SELECT F.PersonId, P1.Name, P1.Lastname, F.RelatedPersonId, P2.Name RelatedName, P2.Lastname RelatedLastName, F.RelationShip
    FROM PersonFamily F
    LEFT JOIN Person P1 ON F.PersonId=P1.Id
    LEFT JOIN Person P2 ON F.RelatedPersonId=P2.Id
);

GO

CREATE PROCEDURE [dbo].[AddChild]
    @ParentId uniqueidentifier,
    @Name varchar(100),
    @Lastname varchar(100),
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
        INSERT INTO Person (Id, Name, LastName, Birthdate, Sex, PhotoFileName) VALUES
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
            1 --Parent
        );
        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip) VALUES
        (
            @childId,
            @ParentId,
            3 --Child
        );

        -- Create relationship between parent-child (the other parent)
        DECLARE @partnerId uniqueidentifier = (SELECT TOP 1 RelatedPersonId FROM PersonFamily WHERE PersonId = @ParentId AND Relationship = 0);
        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip) VALUES
        (
            @partnerId,
            @childId,
            1 --Parent
        );
        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip) VALUES
        (
            @childId,
            @partnerId,
            3 --Child
        );

        -- Create relationship between child and the other brothers (we look for the children of the parent, without the new one)
        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip)
            SELECT @childId, RelatedPersonId, 2 FROM PersonFamily WHERE PersonId = @ParentId AND RelatedPersonId <> @childId AND Relationship = 1;

        INSERT INTO PersonFamily (PersonId, RelatedPersonId, RelationShip)
            SELECT RelatedPersonId, @childId, 2 FROM PersonFamily WHERE PersonId = @ParentId AND RelatedPersonId <> @childId AND Relationship = 1;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
    END CATCH;
END;

GO