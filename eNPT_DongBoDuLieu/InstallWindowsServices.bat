::stop service
::echo "stop service..."
sc stop eNPT_DongBoDuLieu

::delete service
::echo "delete service..."
sc delete eNPT_DongBoDuLieu

::create windows service
::echo "create service..."
sc create "eNPT_DongBoDuLieu" binPath="C:\Program Files\SV Tech\DongBoDuLieuServices\eNPT_DongBoDuLieu.exe"

::config service automatic
::echo "config service automatic..."
sc config eNPT_DongBoDuLieu start=auto

::start service
::echo "start service..."
sc start eNPT_DongBoDuLieu
pause
