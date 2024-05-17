USE [master]
GO

IF EXISTS (SELECT name FROM master.sys.databases WHERE name = 'Hotel')
BEGIN
	PRINT '' PRINT 'Dropping DB'
	DROP DATABASE Hotel
END
GO

CREATE DATABASE Hotel
PRINT '' PRINT 'Creating DB'
GO

USE[Hotel]
PRINT '' PRINT 'Using DB'
GO



PRINT '' PRINT 'Creating Guest table'
GO
/*Creating Guest Table*/
CREATE TABLE [dbo].[Guest](
	[GuestID] [INT] IDENTITY(100000,1)
	, [FirstName] [NVARCHAR](25) NOT NULL
	, [LastName] [NVARCHAR](25) NOT NULL
	, [Phone] [NVARCHAR](25) UNIQUE NOT NULL
	, [Email] [NVARCHAR](100) UNIQUE NOT NULL 
	, [PasswordHash] [NVARCHAR] (100) NOT NULL DEFAULT 'b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342'
	, CONSTRAINT [pk_Guest_GuestID] PRIMARY KEY ([GuestID])
)
GO


PRINT '' PRINT 'Creating Pantry table'
GO
/*Creating Pantry Table*/

CREATE TABLE [dbo].[Pantry](
	[PantryID] [NVARCHAR](50) NOT NULL
	, [ItemPrice] [INT] NOT NULL
	, [ItemAmount] [INT] DEFAULT 1 NOT NULL
	, CONSTRAINT [pk_Pantry_PantryID] PRIMARY KEY ([PantryID])
)
GO

PRINT '' PRINT 'Creating Position table'
GO
/*Creating Position table*/
CREATE TABLE [dbo].[Position](
	[PositionID] [INT] IDENTITY(100000,1) NOT NULL
	, [PositionTitle] [NVARCHAR](25) NOT NULL
	, [PositionDescription] [NVARCHAR](500) NOT NULL
	, CONSTRAINT [pk_Postition_PositionID] PRIMARY KEY ([PositionID])
)
GO

PRINT '' PRINT'Creating RoomType Table'
GO
/*Creating RoomType Table*/
CREATE TABLE [dbo].[RoomType](
	[RoomTypeID] [NVARCHAR](75) NOT NULL
	, [RoomPrice] [INT] NOT NULL
	, [RoomDescription] [NVARCHAR](255) NOT NULL
	, CONSTRAINT [pk_RoomType_RoomTypeID] PRIMARY KEY ([RoomTypeID])
)
GO

PRINT '' PRINT'Creating Room Table'
GO
/*Creating Room Table*/
CREATE TABLE [dbo].[Room](
	[RoomID] [INT] NOT NULL
	, [RoomTypeID] [NVARCHAR](75) NOT NULL
	, [RoomAvailability] [BIT] DEFAULT(1)
	, [RoomStatus] [NVARCHAR](9) NOT NULL DEFAULT 'Inspected'
	, CONSTRAINT [pk_Room_RoomID] PRIMARY KEY ([RoomID])
	, CONSTRAINT [fk_Room_RoomTypeID] FOREIGN KEY ([RoomTypeID]) REFERENCES [dbo].[RoomType]([RoomTypeID])
)
GO


PRINT '' PRINT 'Creating Reservation table'
GO
/*Creating Reservation Table*/
CREATE TABLE [dbo].[Reservation](
	[ReservationID] [INT] IDENTITY(100000,1)
	, [GuestID] [INT] NOT NULL
	, [RoomID] [INT] NOT NULL
	, [ReservationStatus] [NVARCHAR](9) DEFAULT('Due In') NOT NULL
	, [CheckIn] [DATE] NOT NULL
	, [CheckOut] [DATE] NOT NULL
	, [Comments] [TEXT] NULL DEFAULT ''
	, [AdultAmount] [INT] NOT NULL DEFAULT 1
	, [ChildAmount] [INT] NOT NULL DEFAULT 0
	, [Paid] [BIT] NOT NULL DEFAULT 0
	, CONSTRAINT [pk_Reservation_ReservationID] PRIMARY KEY ([ReservationID])
	, CONSTRAINT [fk_Reservation_GuestID] FOREIGN KEY ([GuestID]) REFERENCES [dbo].[Guest]([GuestID])
	, CONSTRAINT [fk_Reservation_RoomID] FOREIGN KEY ([RoomID]) REFERENCES [dbo].[Room]([RoomID])
)
GO


PRINT '' PRINT'Creating RoomCharge Table'
GO
/*Creating RoomCharge Table*/
CREATE TABLE [dbo].[RoomCharge](
	[RoomChargeID] [INT] IDENTITY(100000,1) NOT NULL
	, [ReservationID] [INT] NOT NULL
	, CONSTRAINT [pk_RoomCharge_RoomChargeID] PRIMARY KEY ([RoomChargeID])
	, CONSTRAINT [fk_RoomCharge_ReservationID] FOREIGN KEY ([ReservationID]) REFERENCES [dbo].[Reservation]([ReservationID])
)
GO



PRINT '' PRINT'Creating RoomChargeItem Table'
GO
/*Creating RoomChargeItem Table*/
CREATE TABLE [dbo].[RoomChargeItem](
	[RoomChargeItemID] [INT] IDENTITY(100000,1) NOT NULL
	, [RoomChargeID] [INT] NOT NULL
	, [PantryID] [NVARCHAR](50) NOT NULL
	, [ItemAmount] [INT]
	, [Active] [BIT] NOT NULL DEFAULT (1)
	, CONSTRAINT [pk_RoomChargeItem_RoomChargeItemID] PRIMARY KEY ([RoomChargeItemID])
	, CONSTRAINT [fk_RoomChargeItem_RoomChargeID] FOREIGN KEY ([RoomChargeID]) REFERENCES [dbo].[RoomCharge]([RoomChargeID])
	, CONSTRAINT [fk_RoomChargeItem_PantryID] FOREIGN KEY ([PantryID]) REFERENCES [dbo].[Pantry]([PantryID])
)
GO

PRINT '' PRINT'Creating Employee Table'
GO
/*Creating Employee Table*/
CREATE TABLE [dbo].[Employee] (
	[EmployeeID] [INT] IDENTITY(100000,1) NOT NULL
	, [PositionID] [INT] NOT NULL
	, [FirstName] [NVARCHAR](25) NOT NULL
	, [LastName] [NVARCHAR](25) NOT NULL
	, [Phone] [NVARCHAR](25) UNIQUE NOT NULL
	, [Email] [NVARCHAR](100) UNIQUE NOT NULL
	, [PasswordHash] [NVARCHAR] (100) NOT NULL DEFAULT 'b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342'
	, CONSTRAINT [pk_Employee_EmployeeID] PRIMARY KEY ([EmployeeID])
	, CONSTRAINT [fk_Employee_PositionID] FOREIGN KEY ([PositionID]) REFERENCES [dbo].[Position]([PositionID])
)
GO

PRINT '' PRINT'Creating Sales Log'
GO
/*Creating Sales Log Table*/
CREATE TABLE [dbo].[SalesLog](
	[LogID] [INT] IDENTITY(100000,1) NOT NULL
	, [EmployeeID] [INT]  NOT NULL
	, [GuestID] [INT] NOT NULL
	, [TimeOfSale] [DATETIME] NOT NULL
	, [PantryID] [NVARCHAR](50) NOT NULL
	, [SoldPrice] [INT] NOT NULL
	, [ItemAmount] [INT] NOT NULL
	, CONSTRAINT [pk_SalesLog_LogID] PRIMARY KEY ([LogID])
	, CONSTRAINT [fk_SalesLog_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee]([EmployeeID])
	, CONSTRAINT [fk_SalesLog_GuestID] FOREIGN KEY ([GuestID]) REFERENCES [dbo].[Guest]([GuestID])
	, CONSTRAINT [fk_SalesLog_PantryID] FOREIGN KEY ([PantryID]) REFERENCES [dbo].[Pantry]([PantryID])
)
GO


/*Inserting dummy data*/
PRINT ''
PRINT 'Inserting data into [dbo].[Guest] table'
GO
-- Guests
INSERT INTO [dbo].[Guest] ([FirstName], [LastName], [Phone], [Email])
VALUES
    ('John', 'Smith', '123-456-7890', 'jsmith@email.com'),
    ('Mary', 'Jones', '234-567-8901', 'mjones@email.com'),
    ('Bob', 'Williams', '345-678-9012', 'bwilliams@email.com'),
    ('Matthew', 'Baccam', '111-111-1111', 'matthewbaccam@email.com') 
GO

PRINT ''
PRINT 'Inserting data into [dbo].[Position] table'  
GO
-- Positions
INSERT INTO [dbo].[Position] ([PositionTitle], [PositionDescription])  
VALUES
    ('Housekeeper', 'Cleans rooms'),
    ('Front Desk Agent', 'Checks in and out guests'),
    ('Admin', 'In charge of user administration')
GO

PRINT ''
PRINT 'Inserting data into [dbo].[RoomType] table'
GO
-- Room Types
INSERT INTO [dbo].[RoomType] ([RoomTypeID], [RoomPrice], [RoomDescription])
VALUES
    ('Single', 150, 'One queen bed, mini fridge, microwave'),
    ('Double', 150, 'Two queen beds, fridge with freezer, microwave, stove')
GO

PRINT ''
PRINT 'Inserting data into [dbo].[Room] table'  
GO
-- Rooms
INSERT INTO [dbo].[Room] ([RoomID], [RoomTypeID])
VALUES
    (101, 'Single'),
    (102, 'Double'),
    (103, 'Single'), 
    (104, 'Double'),
    (105, 'Single'),
    (106, 'Double'),
    (107, 'Single'),
    (108, 'Double'),
    (109, 'Single'),
    (110, 'Double')
GO

PRINT ''
PRINT 'Inserting data into [dbo].[Employee] table'
GO
-- Employees
INSERT INTO [dbo].[Employee] ([PositionID], [FirstName], [LastName], [Phone], [Email])
VALUES  
    (100000, 'Jane', 'Doe', '111-222-3333', 'jdoe@hotel.com'),
    (100001, 'John', 'Smith', '444-555-6666', 'jsmith@hotel.com'),
    (100002, 'Admin', 'Adman', '123-123-1234', 'admin@company.com')
GO

PRINT ''
PRINT 'Inserting data into [dbo].[Reservation] table'
GO
-- Reservations for April and May 
INSERT INTO [dbo].[Reservation] ([GuestID], [RoomID], [ReservationStatus], [CheckIn], [CheckOut], [AdultAmount], [ChildAmount])
VALUES
    ('100000', 101, 'Out', '2024-04-05', '2024-04-10', 2, 1),
    ('100001', 102, 'Out', '2024-04-10', '2024-04-15', 2, 0),
    ('100002', 103, 'Out', '2024-04-15', '2024-04-20', 1, 0),
    ('100003', 104, 'Out', '2024-04-20', '2024-04-25', 1, 1),
    ('100002', 105, 'Due Out', '2024-04-01', '2024-04-07', 1, 0),
    ('100003', 106, 'Due In', '2024-04-07', '2024-04-13', 1, 2),
    ('100001', 107, 'Due In', '2024-04-08', '2024-04-13', 1, 1),
    ('100000', 108, 'Due Out', '2024-04-15', '2024-04-20', 2, 1),
    ('100002', 109, 'Due In', '2024-04-18', '2024-04-25', 1, 0),
    ('100003', 110, 'Due Out', '2024-04-21', '2024-04-27', 1, 2)
GO

PRINT ''
PRINT 'Inserting data into [dbo].[Pantry] table'
GO
-- Pantry  
INSERT INTO [dbo].[Pantry] ([PantryID], [ItemPrice], [ItemAmount])
VALUES
    ('Candy', 2, 10),
    ('Chips', 4, 10),
    ('Snack', 2, 10),
    ('Water', 1, 10),
    ('Juice', 2, 10),
    ('Soda', 3, 10)
GO

PRINT ''
PRINT 'Inserting data into [dbo].[RoomCharge] table'  
GO
-- Room Charges
INSERT INTO [dbo].[RoomCharge] ([ReservationID])
VALUES
    (100000)
GO

PRINT '' 
PRINT 'Inserting data into [dbo].[RoomChargeItem] table'
GO
-- Room Charge Items
INSERT INTO [dbo].[RoomChargeItem] ([RoomChargeID], [PantryID], [ItemAmount])
VALUES
    (100000,'Candy', 2),
    (100000,'Chips', 2),
    (100000,'Water', 2)
GO

PRINT '' 
PRINT 'Inserting data into [dbo].[SalesLog] table'
GO
-- Sales Log
INSERT INTO [dbo].[SalesLog] ([EmployeeID], [GuestID], [TimeOfSale], [PantryID], [SoldPrice], [ItemAmount])
VALUES
    (100001, 100000, '2023-11-11 13:23:44', 'Candy', 4, 2),
    (100001, 100000, '2023-11-11 13:23:44', 'Chips', 8, 2),
    (100001, 100000, '2023-11-11 13:23:44', 'Water', 2, 2)
GO

-- STORED PROCEDURES 
PRINT '' 
PRINT 'Creating sp_select_events_by_status'
GO
CREATE PROCEDURE [dbo].[sp_select_events_by_status](
	@EventsStatus [NVARCHAR] (9)
) AS
	BEGIN 
	if(@EventsStatus = 'All')
		SELECT [ReservationID] -- ID
			, [Guest].[FirstName] -- Title
			, [Guest].[LastName] -- Title
			, [CheckIn] -- Start
			, [CheckOut] -- End
			, [ReservationStatus] -- Used to interpret the color 
			FROM [Reservation]
			INNER JOIN [Guest]
			ON [Reservation].[GuestID] = [Guest].[GuestID]
	else 
		SELECT [ReservationID] -- ID
			, [Guest].[FirstName] -- Title
			, [Guest].[LastName] -- Title
			, [CheckIn] -- Start
			, [CheckOut] -- End
			, [ReservationStatus] -- Used to interpret the color 
			FROM [Reservation]
			INNER JOIN [Guest]
			ON [Reservation].[GuestID] = [Guest].[GuestID]
			WHERE @EventsStatus = [ReservationStatus]
		
	END
GO

PRINT '' 
PRINT 'Creating sp_get_reservations_by_status'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_status](
	@ReservationStatus [NVARCHAR] (9)
) AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
			FROM [Reservation]
			WHERE @ReservationStatus = [ReservationStatus]
	END
GO


PRINT '' 
PRINT 'Creating sp_select_guest_reservations_ascending'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_reservations_ascending](
	@GuestID [INT]
)
AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
			FROM [Reservation]
			WHERE [GuestID] = @GuestID
			ORDER BY [CheckIn] ASC
	END
GO


PRINT '' 
PRINT 'Creating sp_select_guest_reservations_descending'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_reservations_descending](
	@GuestID [INT]
)
AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
			FROM [Reservation]
			WHERE [GuestID] = @GuestID
			ORDER BY [CheckIn] DESC
	END
GO


PRINT '' 
PRINT 'Creating sp_select_guest_reservations_by_reservation_status'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_reservations_by_reservation_status](
	@GuestID [INT],
	@ReservationStatus [NVARCHAR](9)
)
AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
			FROM [Reservation]
			WHERE [GuestID] = @GuestID
			AND [ReservationStatus] = @ReservationStatus
		END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_reservationID'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_guestID](
	@GuestID [INT]
) AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
			FROM [Reservation]
			WHERE @GuestID = [Reservation].[GuestID]
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_reservationID'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_reservationID](
	@ReservationID [INT]
) AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
			FROM [Reservation]
			WHERE @ReservationID = [Reservation].[ReservationID]
	END
GO


PRINT '' 
PRINT 'Creating sp_select_room'
GO
CREATE PROCEDURE [dbo].[sp_select_room](
	@RoomID [INT]
) AS
	BEGIN
		SELECT [RoomID]
			, [RoomTypeID]
			, [RoomAvailability]
			, [RoomStatus]
			FROM [Room]
			WHERE @RoomID = [RoomID]
	END
GO

PRINT '' 
PRINT 'Creating sp_select_room_type'
GO
CREATE PROCEDURE [dbo].[sp_select_room_type](
	@RoomTypeID [NVARCHAR](75)
) AS
	BEGIN
		SELECT [RoomTypeID]
			, [RoomPrice]
			, [RoomDescription]
			FROM [RoomType]
			WHERE @RoomTypeID = [RoomTypeID]
	END
GO

PRINT '' 
PRINT 'Creating sp_select_room_charge'
GO
CREATE PROCEDURE [dbo].[sp_select_room_charge](
	@ReservationID [INT]
) AS
	BEGIN
		SELECT  [RoomChargeID],
			[ReservationID]
		FROM [RoomCharge]
	END
GO

PRINT '' 
PRINT 'Creating sp_select_positions'
GO
CREATE PROCEDURE [dbo].sp_select_positions AS
	BEGIN
		SELECT  [PositionID]
			, [PositionTitle]
			, [PositionDescription]
		FROM [Position]
	END
GO

PRINT '' 
PRINT 'Creating sp_select_room_charge_items'
GO
CREATE PROCEDURE [dbo].[sp_select_room_charge_items](
	@RoomChargeID [INT]
) AS
	BEGIN
		SELECT [RoomChargeItemID], 
			[RoomChargeItem].[RoomChargeID],
			[PantryID],
			[ItemAmount],
			[Active]
		FROM [RoomChargeItem]
		WHERE @RoomChargeID = [RoomChargeItem].[RoomChargeID]
	END
GO 

PRINT '' 
PRINT 'Creating sp_select_active_room_charge_items'
GO
CREATE PROCEDURE [dbo].[sp_select_active_room_charge_items](
	@RoomChargeID [INT]
)	
AS
	BEGIN
		SELECT [RoomChargeItemID], 
			[RoomChargeID],
			[PantryID],
			[ItemAmount],
			[Active]
		FROM [RoomChargeItem]
		WHERE [RoomChargeID] = @RoomChargeID AND [Active] = 1
	END
GO 

PRINT '' 
PRINT 'Creating sp_select_pantry_item'
GO
CREATE PROCEDURE [dbo].[sp_select_pantry_item](
	@PantryID [NVARCHAR](50)
) AS
	BEGIN
		SELECT [PantryID], 
			[ItemPrice],
			[ItemAmount]
		FROM [Pantry]
		WHERE [PantryID] = @PantryID
	END
GO

PRINT '' 
PRINT 'Creating sp_select_pantry_items'
GO
CREATE PROCEDURE [dbo].[sp_select_pantry_items]
AS
	BEGIN
		SELECT [PantryID], 
			[ItemPrice],
			[ItemAmount]
		FROM [Pantry]
	END
GO

PRINT '' 
PRINT 'Creating sp_authenticate_room_charges'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_room_charges](
	@ReservationID [INT]
) AS
	BEGIN
		SELECT [RoomCharge].[ReservationID]
		FROM [RoomCharge]
		INNER JOIN [Reservation]
		ON [RoomCharge].[ReservationID] = [Reservation].[ReservationID]
		WHERE @ReservationID = [RoomCharge].[ReservationID]
	END
GO

PRINT '' 
PRINT 'Creating sp_select_guest_by_guestID'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_by_guestID](
	@GuestID [INT]
) AS
	BEGIN
		SELECT [GuestID], 
			[FirstName],
			[LastName],
			[Phone],
			[Email]
		FROM [Guest]
		WHERE @GuestID = [GuestID]
	END
GO

PRINT '' 
PRINT 'Creating sp_update_reservation_comments'
GO
CREATE PROCEDURE [dbo].[sp_update_reservation_comments](
	@ReservationID [INT]
	, @NewComments [TEXT]
) AS
	BEGIN 
		UPDATE [Reservation]
		SET [Comments] = @NewComments
		WHERE [ReservationID] = @ReservationID
	END
GO


PRINT '' 
PRINT 'Creating sp_update_child_adult_amount'
GO
CREATE PROCEDURE [dbo].[sp_update_child_adult_amount](
	@ReservationID [INT]
	, @NewAdultAmount [INT]
	, @NewChildAmount [INT]
	, @OldAdultAmount [INT]
	, @OldChildAmount [INT]
) AS
	BEGIN 
		UPDATE [Reservation]
		SET [AdultAmount] = @NewAdultAmount ,
			[ChildAmount] = @NewChildAmount
		WHERE [ReservationID] = @ReservationID
		AND [AdultAmount] = @OldAdultAmount
		AND [ChildAmount] = @OldChildAmount
	END
GO

PRINT '' 
PRINT 'Creating sp_update_reservation_for_checkin'
GO
CREATE PROCEDURE [dbo].[sp_update_reservation_for_checkin](
	@ReservationID [INT]
	, @NewCheckIn [DATE]
	, @OldCheckIn [DATE]
) AS
	BEGIN 
		DECLARE @ReservationStatus NVARCHAR(8) = 'Due Out'
		UPDATE [Reservation]
		SET [ReservationStatus] = @ReservationStatus
			, [CheckIn] = @NewCheckIn
		WHERE [ReservationID] = @ReservationID
			AND [CheckIn] = @OldCheckIn
	END
GO

PRINT '' 
PRINT 'Creating sp_update_reservation_for_checkout'
GO
CREATE PROCEDURE [dbo].[sp_update_reservation_for_checkout](
	@ReservationID [INT]
	, @NewCheckOut [DATE]
	, @OldCheckOut [DATE]
) AS
	BEGIN 
		DECLARE @ReservationStatus NVARCHAR(3) = 'Out'
		UPDATE [Reservation]
		SET [ReservationStatus] = @ReservationStatus
			, [Paid] = 1
			, [CheckOut] = @NewCheckOut
		WHERE [ReservationID] = @ReservationID
		AND [CheckOut] = @OldCheckOut
	END
GO

PRINT '' 
PRINT 'Creating sp_update_reservation_for_cancel'
GO
CREATE PROCEDURE [dbo].[sp_update_reservation_for_cancel](
	@ReservationID [INT]
	, @OldReservationStatus [NVARCHAR](9)
) AS
	BEGIN 
		DECLARE @NewReservationStatus NVARCHAR(9) = 'Canceled'
		UPDATE [Reservation]
		SET [ReservationStatus] = @NewReservationStatus
		WHERE [ReservationID] = @ReservationID AND [ReservationStatus] = @OldReservationStatus
	END
GO

PRINT '' 
PRINT 'Creating sp_update_room_status'
GO
CREATE PROCEDURE [dbo].[sp_update_room_status](
	@RoomID [INT]
	, @NewRoomStatus [NVARCHAR](9)
	, @OldRoomStatus [NVARCHAR](9)
) AS
	BEGIN 
		UPDATE [Room]
		SET [RoomStatus] = @NewRoomStatus
		WHERE [RoomID] = @RoomID AND [RoomStatus] = @OldRoomStatus
	END
GO

PRINT '' 
PRINT 'Creating sp_update_room_availability'
GO
CREATE PROCEDURE [dbo].[sp_update_room_availability](
	@RoomID [INT]
	, @NewRoomAvailability [BIT]
	, @OldRoomAvailability [BIT]
) AS
	BEGIN 
		UPDATE [Room]
		SET [RoomAvailability] = @NewRoomAvailability
		WHERE [RoomID] = @RoomID AND [RoomAvailability] = @OldRoomAvailability
	END
GO

PRINT '' 
PRINT 'Creating sp_update_reservation'
GO
CREATE PROCEDURE [dbo].[sp_update_reservation](
	@ReservationID [INT]
	, @GuestID [INT]
	, @NewRoomID [INT]
	, @NewCheckIn [DATE]
	, @NewCheckOut [DATE]
	, @NewAdultAmount [INT]
	, @NewChildAmount [INT]
	, @OldRoomID [INT]
	, @OldCheckIn [DATE]
	, @OldCheckOut [DATE]
	, @OldAdultAmount [INT]
	, @OldChildAmount [INT]
) AS
	BEGIN 
		UPDATE [Reservation] 
		SET [GuestID] = @GuestID
		, [RoomID] = @NewRoomID	
		, [CheckIn] = @NewCheckIn	
		, [CheckOut] = @NewCheckOut
		, [AdultAmount] = @NewAdultAmount
		, [ChildAmount] = @NewChildAmount	
		WHERE [ReservationID] = @ReservationID AND
			[GuestID] = @GuestID AND 
			[RoomID] = @OldRoomID	AND
			[CheckIn] = @OldCheckIn AND
			[CheckOut] = @OldCheckOut AND
			[AdultAmount] = @OldAdultAmount AND
			[ChildAmount] = @OldChildAmount
	END
GO

PRINT '' 
PRINT 'Creating sp_update_reservation_reschedule'
GO
CREATE PROCEDURE [dbo].[sp_update_reservation_reschedule](
	@ReservationID [INT]
	, @NewCheckIn [DATE]
	, @NewCheckOut [DATE]
	, @OldCheckIn [DATE]
	, @OldCheckOut [DATE]
) AS
	BEGIN 
		UPDATE [Reservation] 
		SET [CheckIn] = @NewCheckIn	
		, [CheckOut] = @NewCheckOut
		WHERE [ReservationID] = @ReservationID AND	
			[CheckIn] = @OldCheckIn	AND
			[CheckOut] = @OldCheckOut
	END
GO

PRINT '' 
PRINT 'Creating sp_update_reservation_reschedule_new_room'
GO
CREATE PROCEDURE [dbo].[sp_update_reservation_reschedule_new_room](
	@ReservationID [INT]
	, @NewRoomID [INT]
	, @NewCheckIn [DATE]
	, @NewCheckOut [DATE]
	, @OldCheckIn [DATE]
	, @OldCheckOut [DATE]
	, @OldRoomID [INT]
) AS
	BEGIN 
		UPDATE [Reservation] 
		SET [CheckIn] = @NewCheckIn	
		, [CheckOut] = @NewCheckOut
		, [RoomID] = @NewRoomID
		WHERE [ReservationID] = @ReservationID AND	
			[CheckIn] = @OldCheckIn	AND
			[CheckOut] = @OldCheckOut AND
			[RoomID] = @OldRoomID
	END
GO

PRINT '' 
PRINT 'Creating sp_update_guest'
GO
CREATE PROCEDURE [dbo].[sp_update_guest](
	@GuestID [INT]
	, @NewFirstName [NVARCHAR](25)
	, @NewLastName [NVARCHAR](25)
	, @NewPhone [NVARCHAR](25)
	, @NewEmail [NVARCHAR](100)
	, @OldFirstName [NVARCHAR](25)
	, @OldLastName [NVARCHAR](25)
	, @OldPhone [NVARCHAR](25)
	, @OldEmail [NVARCHAR](100)
) AS
	BEGIN 
		UPDATE [Guest]
		SET [FirstName] = @NewFirstName
		, [LastName] = @NewLastName
		, [Phone] = @NewPhone
		, [Email] = @NewEmail
		WHERE [GuestID] = @GuestID AND
			[FirstName] = @OldFirstName AND
			 [LastName] = @OldLastName AND
			 [Phone] = @OldPhone AND
			 [Email] = @OldEmail
	END
GO


PRINT '' 
PRINT 'Creating sp_update_employee'
GO
CREATE PROCEDURE [dbo].[sp_update_employee](
	@EmployeeID [INT]
	, @NewPositionID [INT]
	, @NewFirstName [NVARCHAR](25)
	, @NewLastName [NVARCHAR](25)
	, @NewPhone [NVARCHAR](25)
	, @NewEmail [NVARCHAR](100)
	, @OldPositionID [INT]
	, @OldFirstName [NVARCHAR](25)
	, @OldLastName [NVARCHAR](25)
	, @OldPhone [NVARCHAR](25)
	, @OldEmail [NVARCHAR](100)
) AS
	BEGIN 
		UPDATE [Employee]
		SET [PositionID] = @NewPositionID
		, [FirstName] = @NewFirstName
		, [LastName] = @NewLastName
		, [Phone] = @NewPhone
		, [Email] = @NewEmail
		WHERE [EmployeeID] = @EmployeeID AND
			[PositionID] = @OldPositionID AND
			[FirstName] = @OldFirstName AND
			 [LastName] = @OldLastName AND
			 [Phone] = @OldPhone AND
			 [Email] = @OldEmail
	END
GO


PRINT '' 
PRINT 'Creating sp_select_guest_by_firstname'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_by_firstname](
	@FirstName [NVARCHAR](25)
) AS
	BEGIN 
		SELECT [GuestID]
			, [FirstName]
			, [LastName]
			, [Phone]
			, [Email]
		FROM [Guest]
		WHERE [FirstName] LIKE '%' + @FirstName + '%' 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_guest_by_firstname_lastname'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_by_firstname_lastname](
	@FirstName [NVARCHAR](25),
	@LastName [NVARCHAR](25)
) AS
	BEGIN 
		SELECT [GuestID]
			, [FirstName]
			, [LastName]
			, [Phone]
			, [Email]
		FROM [Guest]
		WHERE [FirstName] LIKE '%' + @FirstName + '%' 
		AND [LastName] LIKE '%' + @LastName + '%' 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_guest_by_email'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_by_email](
	@Email [NVARCHAR](25)
) AS
	BEGIN 
		SELECT [GuestID]
			, [FirstName]
			, [LastName]
			, [Phone]
			, [Email]
		FROM [Guest]
		WHERE [Email] = @Email 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_guest_by_phone'
GO
CREATE PROCEDURE [dbo].[sp_select_guest_by_phone](
	@Phone [NVARCHAR](25)
) AS
	BEGIN 
		SELECT [GuestID]
			, [FirstName]
			, [LastName]
			, [Phone]
			, [Email]
		FROM [Guest]
		WHERE [Phone] = @Phone 
	END
GO

PRINT '' 
PRINT 'Creating sp_insert_guest'
GO
CREATE PROCEDURE [dbo].[sp_insert_guest](
	@FirstName [NVARCHAR](25)
	, @LastName [NVARCHAR](25)
	, @Phone [NVARCHAR](25),
	@Email [NVARCHAR](100)
) AS
	BEGIN 
		INSERT INTO [Guest] (
		[FirstName]
		, [LastName]
		, [Phone]		
		, [Email]
		)
		VALUES (
			@FirstName
			, @LastName
			, @Phone
			, @Email	
		)
	END
GO


PRINT '' 
PRINT 'Creating sp_select_rooms_available_for_reschedule_except'
GO
CREATE PROCEDURE [dbo].[sp_select_rooms_available_for_reschedule_except](
	@CheckIn [DATE]
	,@CheckOut [DATE]
	, @ReservationID [INT]
) AS
	BEGIN
			SELECT DISTINCT [RoomID]
				, [RoomTypeID]
				, [RoomAvailability]
				, [RoomStatus]
			FROM [Room] 
			WHERE [RoomID] NOT IN (
			SELECT [RoomID]
			FROM [Reservation]
			WHERE ([ReservationStatus] = 'Due In' OR [ReservationStatus] = 'Due Out')
			AND (
				(@CheckIn >= [Reservation].[CheckIn] AND @CheckIn <= [Reservation].[CheckOut])
				OR 
				(@CheckOut >= [Reservation].[CheckIn] AND @CheckOut <= [Reservation].[CheckOut])
			)
			AND [ReservationID] != @ReservationID
		)
	END
GO

PRINT '' 
PRINT 'Creating sp_select_rooms_available'
GO
CREATE PROCEDURE [dbo].[sp_select_rooms_available](
	@CheckIn [DATE]
	,@CheckOut [DATE]
) AS
	BEGIN
			SELECT DISTINCT [RoomID]
				, [RoomTypeID]
				, [RoomAvailability]
				, [RoomStatus]
			FROM [Room] 
			WHERE [RoomID] NOT IN (
			SELECT [RoomID]
			FROM [Reservation]
			WHERE ([ReservationStatus] = 'Due In' OR [ReservationStatus] = 'Due Out')
			AND (
				(@CheckIn >= [Reservation].[CheckIn] AND @CheckIn <= [Reservation].[CheckOut])
				OR 
				(@CheckOut >= [Reservation].[CheckIn] AND @CheckOut <= [Reservation].[CheckOut])
			)
		)
	END
GO

PRINT '' 
PRINT 'Creating sp_insert_reservation'
GO
CREATE PROCEDURE [dbo].[sp_insert_reservation](
	  @GuestID [INT]
	, @RoomID [INT]
	, @ReservationStatus [NVARCHAR](9)
	, @CheckIn [Date]
	, @CheckOut [Date]
	, @Comments [Text]
	, @AdultAmount [INT]
	, @ChildAmount [INT]
	, @Paid [BIT]
) AS
	BEGIN 
		INSERT INTO [Reservation] (
			[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		)
		VALUES (
			@GuestID
			, @RoomID
			, @ReservationStatus
			, @CheckIn
			, @CheckOut
			, @Comments
			, @AdultAmount
			, @ChildAmount
			, @Paid
		)
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_all'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_all](
	@FirstName [NVARCHAR](25)
	, @LastName [NVARCHAR](25)
	, @CheckIn [DATE]
	, @CheckOut [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%' 
			AND [Guest].[LastName] LIKE '%' +  @LastName + '%' 
			AND [CheckIn] = @CheckIn 
			AND [CheckOut] = @CheckOut
	END
GO


PRINT '' 
PRINT 'Creating sp_select_reservations_by_roomID'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_roomID](
	@RoomID [INT]
) AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		WHERE [RoomID] = @RoomID 
			AND [ReservationStatus] = 'Due Out' 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_firstname_checkin_checkout'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_firstname_checkin_checkout](
	@FirstName [NVARCHAR](25)
	, @CheckIn [DATE]
	, @CheckOut [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%'
			AND [CheckIn] = @CheckIn 
			AND [CheckOut] = @CheckOut 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_firstname_lastname_roomID'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_firstname_lastname_roomID](
	@FirstName [NVARCHAR](25)
	, @LastName [NVARCHAR](25)
	, @RoomID [INT]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%'
			AND [Guest].[LastName] LIKE '%' + @FirstName + '%'
			AND [RoomID] = @RoomID
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_firstname_lastname'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_firstname_lastname](
	@FirstName [NVARCHAR](25)
	, @LastName [NVARCHAR](25)
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%'
			AND [Guest].[LastName] LIKE '%' + @FirstName + '%'
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_lastname_checkin_checkout'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_last_name_check_in_check_out](
	@LastName [NVARCHAR](25)
	, @CheckIn [DATE]
	, @CheckOut [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE Guest.[LastName] LIKE '%' + @LastName + '%'
			AND [CheckIn] = @CheckIn 
			AND [CheckOut] = @CheckOut 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_checkin_checkout'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_checkin_checkout](
	@CheckIn [DATE]
	, @CheckOut [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		WHERE [CheckIn] = @CheckIn 
			AND [CheckOut] = @CheckOut 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_checkin'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_checkin](
	@CheckIn [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		WHERE [CheckIn] = @CheckIn 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_checkout'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_checkout](
	@CheckOut [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		WHERE [CheckOut] = @CheckOut 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_firstname_lastname'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_firstname_lastname](
	@FirstName [NVARCHAR](25)
	, @LastName [NVARCHAR](25)
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%'
			AND [Guest].[LastName] LIKE '%' + @LastName + '%'
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_firstname'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_firstname](
	@FirstName [NVARCHAR](25)
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%'
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_lastname'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_lastname](
	@LastName [NVARCHAR](25)
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[LastName] LIKE '%' + @LastName + '%'
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_firstname_checkin'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_firstname_checkin](
	@FirstName [NVARCHAR](25)
	, @CheckIn [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%'
			AND [CheckIn] = @CheckIn 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_firstname_checkout'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_firstname_checkout](
	@FirstName [NVARCHAR](25)
	, @CheckOut [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[FirstName] LIKE '%' + @FirstName + '%'
			AND [CheckOut] = @CheckOut 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_lastname_checkin'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_lastname_checkin](
	@LastName [NVARCHAR](25)
	, @CheckIn [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[LastName] LIKE '%' + @LastName + '%'
			AND [CheckIn] = @CheckIn 
	END
GO

PRINT '' 
PRINT 'Creating sp_select_reservations_by_search_lastname_checkout'
GO
CREATE PROCEDURE [dbo].[sp_select_reservations_by_search_lastname_checkout](
	@LastName [NVARCHAR](25)
	, @CheckOut [DATE]
) AS
	BEGIN
		SELECT [ReservationID]
			, [Reservation].[GuestID]
			, [RoomID]
			, [ReservationStatus]
			, [CheckIn]
			, [CheckOut]
			, [Comments]
			, [AdultAmount]
			, [ChildAmount]
			, [Paid]
		FROM [Reservation]
		INNER JOIN [Guest]
		ON [Reservation].[GuestID] = [Guest].[GuestID]
		WHERE [Guest].[LastName] LIKE '%' + @LastName + '%'
			AND [CheckOut] = @CheckOut 
	END
GO

PRINT '' 
PRINT 'Creating sp_create_room_charge'
GO
CREATE PROCEDURE [dbo].[sp_create_room_charge](
	@ReservationID [INT]
) AS
	BEGIN
		INSERT [RoomCharge] ([ReservationID])
		VALUES(@ReservationID)
	END
GO

PRINT '' 
PRINT 'Creating sp_insert_room_charge_items'
GO
CREATE PROCEDURE [dbo].[sp_insert_room_charge_items](
	@RoomChargeID [INT]
	, @PantryID [NVARCHAR](50)
	, @ItemAmount [INT]
) AS
	BEGIN
		INSERT [RoomChargeItem] ([RoomChargeID], [PantryID], [ItemAmount])
		VALUES(@RoomChargeID ,@PantryID, @ItemAmount)
	END
GO

PRINT '' 
PRINT 'Creating sp_update_room_charge_items_for_refund'
GO
CREATE PROCEDURE [dbo].[sp_update_room_charge_items_for_refund](
	@RoomChargeItemID [INT],
	@RoomChargeID [INT],
	@NewActive [BIT],
	@OldActive [BIT]
) AS
	BEGIN
		UPDATE [RoomChargeItem]
		SET [Active] = @NewActive
		WHERE [RoomChargeItemID] = @RoomChargeItemID
		AND [Active] = @OldActive
		AND [RoomChargeID] = @RoomChargeID
	END
GO


PRINT '' 
PRINT 'Creating sp_update_pantry_quantity'
GO
CREATE PROCEDURE [dbo].[sp_update_pantry_quantity](
	@PantryID [NVARCHAR](50)
	, @NewItemAmount [INT]
	, @OldItemAmount [INT]
) AS
	BEGIN
		UPDATE [Pantry]
		SET [ItemAmount] -= @NewItemAmount
		WHERE [PantryID] = @PantryID
		AND [ItemAmount] = @OldItemAmount
	END
GO

PRINT '' 
PRINT 'Creating sp_update_pantry_quantity_and_price'
GO
CREATE PROCEDURE [dbo].[sp_update_pantry_quantity_and_price](
	@PantryID [NVARCHAR](50)
	, @NewItemAmount [INT]
	, @NewItemPrice [INT]
	, @OldItemAmount [INT]
	, @OldItemPrice [INT]
) AS
	BEGIN
		UPDATE [Pantry]
		SET [ItemAmount] = @NewItemAmount , 
		[ItemPrice] = @NewItemPrice
		WHERE [PantryID] = @PantryID AND
			[ItemAmount] = @OldItemAmount AND
			[ItemPrice] = @OldItemPrice
	END
GO

PRINT '' 
PRINT 'Creating sp_insert_into_saleslog'
GO
CREATE PROCEDURE [dbo].[sp_insert_into_saleslog](
	@EmployeeID [INT]
	, @GuestID [INT]
	, @TimeOfSale [DATETIME]
	, @PantryID [NVARCHAR](50)
	, @SoldPrice [INT]
	, @ItemAmount [INT]
) AS
	BEGIN
		INSERT [SalesLog] 
			(
			 [EmployeeID] 
			,[GuestID]
			,[TimeOfSale]
			,[PantryID]
			,[SoldPrice]
			,[ItemAmount]
			)
		Values(
				@EmployeeID
				, @GuestID
				, @TimeOfSale
				, @PantryID
				, @SoldPrice
				, @ItemAmount
				)
	END
GO

PRINT '' 
PRINT 'Creating sp_select_saleslog'
GO
CREATE PROCEDURE [dbo].[sp_select_saleslog]
AS
	BEGIN
		SELECT  [LogID]
				,[EmployeeID]
				, [GuestID]
				, [TimeOfSale]
				, [PantryID]
				, [SoldPrice]
				, [ItemAmount]
		FROM [SalesLog]
	END
GO

PRINT '' 
PRINT 'Creating sp_select_rooms_by_status'
GO
CREATE PROCEDURE [dbo].[sp_select_rooms_by_status](
	@RoomStatus [NVARCHAR](9)
)
AS
	BEGIN
		SELECT [RoomID]
				, [RoomTypeID]
				, [RoomAvailability]
				, [RoomStatus]
		FROM [Room]
		WHERE [RoomStatus] = @RoomStatus
	END
GO

PRINT '' 
PRINT 'Creating sp_select_all_rooms'
GO
CREATE PROCEDURE [dbo].[sp_select_all_rooms]
AS
	BEGIN
		SELECT [RoomID]
				, [RoomTypeID]
				, [RoomAvailability]
				, [RoomStatus]
		FROM [Room]
	END
GO

PRINT '' 
PRINT 'Creating sp_update_room'
GO
CREATE PROCEDURE [dbo].[sp_update_room](
	@RoomID [INT],
	@NewRoomTypeID [NVARCHAR](75),
	@NewRoomAvailability [BIT],
	@NewRoomStatus [NVARCHAR](9),
	@OldRoomTypeID [NVARCHAR](75),
	@OldRoomAvailability [BIT],
	@OldRoomStatus [NVARCHAR](9)
)
AS
	BEGIN
		UPDATE [Room]
		SET 
		[RoomTypeID] = @NewRoomTypeID ,
		[RoomAvailability] = @NewRoomAvailability ,
		[RoomStatus] = @NewRoomStatus
		WHERE 
		[RoomID] = @RoomID AND
		[RoomTypeID] = @OldRoomTypeID AND
		[RoomAvailability] = @OldRoomAvailability AND
		[RoomStatus] = @OldRoomStatus
	END
GO

PRINT '' 
PRINT 'Creating sp_update_room_type'
GO
CREATE PROCEDURE [dbo].[sp_update_room_type](
	@RoomTypeID [NVARCHAR](75),
	@NewRoomPrice [INT],
	@NewRoomDescription [NVARCHAR](255),
	@OldRoomPrice [INT],
	@OldRoomDescription [NVARCHAR](255)
)
AS
	BEGIN
		UPDATE [RoomType]
		SET
		[RoomPrice] = @NewRoomPrice ,
		[RoomDescription] = @NewRoomDescription
		WHERE 
		[RoomTypeID] = @RoomTypeID AND
		[RoomPrice] = @OldRoomPrice AND
		[RoomDescription] = @OldRoomDescription
	END
GO

PRINT '' 
PRINT 'Creating sp_update_sales_for_refund'
GO
CREATE PROCEDURE [dbo].[sp_update_sales_for_refund](
	@LogID [INT],
	@NewSoldPrice [INT],
	@OldSoldPrice [INT]
)
AS
	BEGIN
		UPDATE [SalesLog]
		SET
		[SoldPrice] = @NewSoldPrice
		WHERE 
		[LogID] = @LogID AND
		[SoldPrice] = @OldSoldPrice
	END
GO

print '' print 'creating sp_authenticate_employee'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_employee](
	@Email	[NVARCHAR](100)
	, @PasswordHash [NVARCHAR](100)
)
AS
	BEGIN	
		SELECT COUNT([EmployeeID]) as 'Authenticated'
		FROM [Employee]
		WHERE @Email = [Email]
			AND @PasswordHash = [PasswordHash]
	END
GO


print '' print 'creating sp_authenticate_guest'
GO
CREATE PROCEDURE [dbo].sp_authenticate_guest(
	@Email	[NVARCHAR](100)
	, @PasswordHash [NVARCHAR](100)
)
AS
	BEGIN	
		SELECT COUNT([GuestID]) as 'Authenticated'
		FROM [Guest]
		WHERE @Email = [Email]
			AND @PasswordHash = [PasswordHash]
	END
GO

print '' print 'creating sp_select_employee_by_email'
GO
CREATE PROCEDURE [dbo].[sp_select_employee_by_email](
	@Email	[NVARCHAR](100)
)
AS
	BEGIN	
		SELECT [PositionID], [EmployeeID] , [FirstName], [LastName], [Phone], [Email]
		FROM [Employee]
		WHERE @Email = [Email]
	END
GO


/*Select Employee by EmployeeID*/
print '' print 'creating sp_select_employee_by_id'
GO
CREATE PROCEDURE [dbo].[sp_select_employee_by_id](
	@EmployeeID	[INT]
)
AS
	BEGIN	
		SELECT [EmployeeID], [PositionID] , [FirstName], [LastName], [Phone], [Email]
		FROM [Employee]
		WHERE @EmployeeID = [EmployeeID]
	END
GO


print '' print 'creating sp_update_passwordhash'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordhash](
	@EmployeeID [INT]
	, @NewPasswordHash [NVARCHAR](100)
	, @OldPassWordHash [NVARCHAR](100)
)
AS
	BEGIN	
		UPDATE [Employee]
		SET [PasswordHash] = @NewPasswordHash
		FROM [Employee]
		WHERE @EmployeeID = [EmployeeID]
			AND @OldPassWordHash = [PasswordHash]
		RETURN @@ROWCOUNT
	END
GO

print '' print 'creating sp_insert_employee'
GO
CREATE PROCEDURE [dbo].[sp_insert_employee](
	@PositionID [NVARCHAR](25)
	, @FirstName [NVARCHAR](25)
	, @LastName [NVARCHAR](25)
	, @Phone [NVARCHAR](25)
	, @Email [NVARCHAR](100)
)
AS
	BEGIN	
		INSERT INTO [Employee] (
		[PositionID]
		, [FirstName]
		, [LastName]
		, [Phone]		
		, [Email]
		)
		VALUES (
			@PositionID
			, @FirstName
			, @LastName
			, @Phone
			, @Email	
		)
	END
GO