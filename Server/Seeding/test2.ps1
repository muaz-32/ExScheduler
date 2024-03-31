for(int i = 2; i <= 6; i++){ 
    for(int j = 1; j < 8; j = j + 2){
            # sent a web request to https://localhost:7227/api/Admin/validatecr/:id
            $url = "https://localhost:7227/api/Admin/validatecr/" + $i + $j
            $response = Invoke-WebRequest -Uri $url -Method Get -Headers @{"Content-Type"="application/json"}
            $response.Content | Out-File -FilePath "C:\Users\user\Desktop\Seeding\test2.txt" -Append
}