CREATE OR ALTER PROCEDURE dbo.Usp_UpdateMemberDetails
(
	@MemberId		INT,
	@MemberFName	VARCHAR(MAX),
	@MemberLName	VARCHAR(MAX),
	@MemberMName	VARCHAR(MAX),
	@Address		VARCHAR(MAX),
	@DOB			DATETIME,
	@Age			VARCHAR(MAX),
	@Contactno		VARCHAR(MAX),
	@EmailID		VARCHAR(MAX),
	@Gender			VARCHAR(MAX),
	@ModifiedBy		BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE DBO.MemberRegistration 
		SET MemberFName=@MemberFName,
			MemberLName=@MemberLName,
			MemberMName=@MemberMName,
			[Address]=@Address,
			DOB=@DOB,
			Age=@Age,
			Contactno=@Contactno,
			EmailID=@EmailID,
			Gender=@Gender,
			ModifiedBy=@ModifiedBy
			--,ModifyDate=GETDATE()
	WHERE MemberID= @MemberID
	SET NOCOUNT OFF;
END