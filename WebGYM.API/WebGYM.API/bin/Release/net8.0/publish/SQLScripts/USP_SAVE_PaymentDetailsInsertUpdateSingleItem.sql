CREATE OR ALTER PROCEDURE dbo.USP_SAVE_PaymentDetailsInsertUpdateSingleItem
(
	@PaymentID		INT,
	@PlanID			INT,
	@WorkouttypeID	INT,
	@Paymenttype	VARCHAR(MAX),
	@PaymentFromdt	DATETIME,
	@PaymentAmount	DECIMAL(18,2),
	@CreateUserID	BIGINT,
	@ModifyUserID	BIGINT,
	@RecStatus		VARCHAR(MAX),
	@MemberID		INT,
	@PaymentIDOUT	INT OUT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @MemberNo VARCHAR(MAX)= (SELECT TOP 1 MemberNo FROM MemberRegistration WHERE MemberID= @MemberID)
	INSERT INTO dbo.PaymentDetails(PlanID,WorkouttypeID,Paymenttype,PaymentFromdt,PaymentAmount,NextRenwalDate,CreateDate,Createdby,ModifyDate,ModifiedBy,RecStatus,MemberID,MemberNo,PaymentTodt)
	SELECT @PlanID PlanID,@WorkouttypeID WorkouttypeID,@Paymenttype Paymenttype,@PaymentFromdt PaymentFromdt,@PaymentAmount PaymentAmount,
	GETDATE()+30 NextRenwalDate,GETDATE() CreateDate,@CreateUserID Createdby,NULL ModifyDate,@ModifyUserID ModifiedBy,@RecStatus RecStatus,@MemberID MemberID,@MemberNo MemberNo,GETDATE()+30  PaymentTodt
	SET @PaymentIDOUT=SCOPE_IDENTITY();
	SET NOCOUNT OFF;
 
END
 