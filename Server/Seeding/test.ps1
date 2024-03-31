# Array of JSON objects
$jsonArray = @(
'{"studentId": 71,"studentName": "sTVE1","studentEmail": "sTVE1@g.com","programeSemester": "TVE 1","studentPassword": "123","studentConfirmPassword": "123", "salt": "123"}',
'{"studentId": 73,"studentName": "sTVE3","studentEmail": "sTVE3@g.com","programeSemester": "TVE 3","studentPassword": "123","studentConfirmPassword": "123", "salt": "123"}'
)

# Send POST request for each JSON object
foreach ($jsonObject in $jsonArray) {
    Invoke-WebRequest -Uri "https://localhost:7227/api/Student/CRSignup" -Method Post -Headers @{"Content-Type"="application/json"} -Body $jsonObject
}
