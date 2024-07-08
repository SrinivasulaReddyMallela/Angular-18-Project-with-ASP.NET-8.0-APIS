CREATE OR ALTER PROCEDURE dbo.USP_PlanMasterInsertUpdateSingleItem
(
	@PlanID				INT,
	@SchemeID			INT,
	@PeriodID			INT,
	@PlanName			VARCHAR(MAX),
	@PlanAmount			DECIMAL(18,2),
	@ServiceTax			VARCHAR(MAX),
	@CreateUserID		BIGINT,
	@ModifyUserID		BIGINT,
	@RecStatus			VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;
	IF ISNULL(@PlanID,0)=0
	BEGIN
		INSERT INTO dbo.PlanMaster(PlanName,PlanAmount,ServicetaxAmount,ServiceTax,CreateDate,CreateUserID,ModifyDate,ModifyUserID,RecStatus,SchemeID,PeriodID,TotalAmount,ServicetaxNo)
		SELECT @PlanName PlanName,@PlanAmount PlanAmount,ISNULL(@PlanAmount,0)*0.18 ServicetaxAmount,@ServiceTax ServiceTax,GETDATE() CreateDate,@CreateUserID CreateUserID,NULL ModifyDate,0 ModifyUserID,
		@RecStatus RecStatus,@SchemeID SchemeID,@PeriodID PeriodID,NULL TotalAmount,'test' ServicetaxNo
		SET @PlanID =SCOPE_IDENTITY();
		UPDATE dbo.PlanMaster 
			SET ServicetaxNo = 'AA' + RIGHT('0000' + CAST(@PlanID AS VARCHAR(4)), 4),
				TotalAmount =PlanAmount+ ServicetaxAmount
		WHERE PlanID=@PlanID
	END
	ELSE
	BEGIN
		UPDATE dbo.PlanMaster 
			SET  SchemeID = @SchemeID,
				 PeriodID=@PeriodID,
				 PlanName=@PlanName,
				 PlanAmount=@PlanAmount,
				 ServiceTax= @ServiceTax,
				 ServicetaxAmount=ISNULL(@PlanAmount,0)*0.18,
				TotalAmount =PlanAmount+ ServicetaxAmount,
				RecStatus=@RecStatus,
				ModifyUserID=@ModifyUserID,
				ModifyDate=GETDATE()
		WHERE PlanID=@PlanID
	END
	SET NOCOUNT OFF;
END

 