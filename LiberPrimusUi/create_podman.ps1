Write-Host "Init the machine"
podman machine stop
podman machine rm -f
podman machine init
podman machine start

Write-Host "Bringing up the initial containers"
podman compose --file docker-compose.yml up --detach