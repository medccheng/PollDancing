WITH XMLVotes AS (
    SELECT
        CONVERT(XML, REPLACE(RecordedVotes, '<?xml version="1.0" encoding="UTF-8"?>', ''), 2) AS VoteDataXML,
        LegislationId,
        ActionDate
    FROM
        dbo.Actions
    WHERE
        ActionDate = (
            SELECT MAX(ActionDate) 
            FROM dbo.Actions A2 
            WHERE A2.LegislationId = dbo.Actions.LegislationId AND A2.RecordedVotes IS NOT NULL
        ) AND ActionCode IN ('8000', '9000')
        AND IsParsed = 0
)

INSERT INTO MemberLegislationVotes (LegislationId, MemberId, Vote)
SELECT
    L.Id as LegislationId, 
    M.Id as MemberId,
    x.Rec.value('(vote/text())[1]', 'varchar(10)') AS Vote
FROM
    XMLVotes AS xVotes
CROSS APPLY
    xVotes.VoteDataXML.nodes('/rollcall-vote/vote-data/recorded-vote') AS x(Rec)
JOIN 
    Members AS M 
    ON M.BioguideId = x.Rec.value('(legislator/@name-id)[1]', 'varchar(100)')
JOIN
    Legislations AS L
    ON L.Id = xVotes.LegislationId
WHERE NOT EXISTS (
    SELECT 1
    FROM MemberLegislationVotes MLV
    WHERE MLV.LegislationId = L.Id AND MLV.MemberId = M.Id
)
