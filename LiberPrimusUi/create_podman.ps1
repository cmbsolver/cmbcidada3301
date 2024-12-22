Write-Host "Init the podman machine"
podman machine stop
podman machine rm -f
podman machine init
podman machine start

Write-Host "Bringing up db container"
podman compose --file docker-compose.yml up --detach