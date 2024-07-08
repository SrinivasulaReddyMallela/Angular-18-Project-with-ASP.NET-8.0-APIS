CREATE OR ALTER PROCEDURE dbo.PICG_MemberRegistrationSelectSingleItem
(
	@MemberId INT
)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 
	SET QUOTED_IDENTIFIER OFF      
	SET NOCOUNT ON  

	SELECT M.MemberId ,
		   M.MemberFName,
		   M.MemberLName,
		   M.MemberMName,
		   M.Dob,
		   M.Age,
		   M.Contactno,
		   M.EmailId,
		   M.Gender,
		   M.PlanID,
		   M.SchemeID,
		   M.JoiningDate,
		   M.Address,
		   PD.PaymentAmount Amount,
		   SM.SchemeName,
		   PM.PlanName
		FROM MemberRegistration M WITH(NOLOCK) 
			INNER JOIN PlanMaster PM WITH(NOLOCK) ON PM.PLANID= M.PLANID
			INNER JOIN SchemeMaster SM WITH(NOLOCK) ON SM.SchemeID= M.SchemeID
			LEFT JOIN PaymentDetails PD WITH(NOLOCK) ON PD.PLANID= M.PLANID AND PD.MemberId= M.MemberId
	WHERE M.MemberId=@MemberId

	SET QUOTED_IDENTIFIER ON      
	SET NOCOUNT OFF
END