CREATE OR ALTER PROCEDURE dbo.picd_MemberRegistrationDeleteSingleItem
(
	@MemberId INT
)
AS
BEGIN
	DELETE FROM PaymentDetails WHERE MemberId =@MemberId
	DELETE FROM MemberRegistration WHERE MemberId =@MemberId
END

