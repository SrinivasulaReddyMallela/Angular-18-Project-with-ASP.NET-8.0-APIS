CREATE OR ALTER PROCEDURE dbo.USP_SAVE_MemberRegistrationInsertUpdateSingleItem
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
	@PlanID			INT,
	@SchemeID		INT,
	@Createdby		bigint,
	@JoiningDate	DATETIME,
	@ModifiedBy		bigint,
	@MemIDOUT		INT OUT
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO DBO.MemberRegistration(MemberFName,MemberLName,MemberMName,Dob,Age,Contactno,EmailId,Gender,PlanID,SchemeID,Createdby,
	ModifiedBy,JoiningDate,[Address],MainMemberId,MemImagePath,MemImagename,MemberNo)

	SELECT @MemberFName MemberFName,@MemberLName MemberLName,@MemberMName MemberMName,@DOB Dob,@Age Age,@Contactno Contactno,@EmailID EmailId,@Gender Gender,@PlanID PlanID,@SchemeID SchemeID,
	@Createdby Createdby,@ModifiedBy	ModifiedBy,@JoiningDate JoiningDate,@Address [Address],0 MainMemberId,'test' MemImagePath,'test' MemImagename,'MemberNo' MemberNo

	SET @MemIDOUT = SCOPE_IDENTITY();
	IF ISNULL(@MemIDOUT,0)<>0
		UPDATE DBO.MemberRegistration 
			SET  MemberNo= 'AA' + RIGHT('0000' + CAST(@MemIDOUT AS VARCHAR(4)), 4)
		WHERE MemberId =@MemIDOUT

	SET NOCOUNT OFF;
END