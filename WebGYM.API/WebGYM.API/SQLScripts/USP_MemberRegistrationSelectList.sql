CREATE OR ALTER PROCEDURE dbo.USP_MemberRegistrationSelectList
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 
	SET QUOTED_IDENTIFIER OFF      
	SET NOCOUNT ON  

	SELECT M.MemberId ,M.MemberNo,
	  ISNULL(M.MemberFName,'')
+	(CASE WHEN ISNULL(M.MemberLName,'') ='' THEN '' ELSE ' ' END)
+	ISNULL(M.MemberLName,'') + 
(CASE WHEN ISNULL(M.MemberMName,'') ='' THEN '' ELSE ' ' END )+ISNULL(M.MemberMName,'')  AS MemberName,
	   M.Dob,
	   M.Contactno,
	   M.EmailId,
	   M.JoiningDate,
	   PD.PaymentAmount Amount,
	   SM.SchemeName,
	   PM.PlanName
	FROM MemberRegistration M WITH(NOLOCK) 
		INNER JOIN PlanMaster PM WITH(NOLOCK) ON PM.PLANID= M.PLANID
		INNER JOIN SchemeMaster SM WITH(NOLOCK) ON SM.SchemeID= M.SchemeID
		LEFT JOIN PaymentDetails PD WITH(NOLOCK) ON PD.PLANID= M.PLANID AND PD.MemberId= M.MemberId
	
	SET QUOTED_IDENTIFIER ON      
	SET NOCOUNT OFF
END
 