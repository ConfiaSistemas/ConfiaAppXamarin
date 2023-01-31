
<?php
include 'DBConfig.php';
 
// Create connection
$conn = new mysqli($HostName, $HostUser, $HostPass, $DatabaseName);
 
if ($conn->connect_error) {
 
 die("Connection failed: " . $conn->connect_error);
} 
$v1=$_GET['usr'];
$sql = "SELECT Usr.id,Usr.nm,Usr.pass,Usr.nm_complete,Usr.addr,TO_BASE64(Usr.img) as img,Usr.imgstr,Usr.tlf,Usr.fecha_alta,Usr.Activo,Empresas.DDNS FROM Usr inner join Empresas on Usr.EmpresaAsignada = Empresas.id where nm = '$v1' ";
 
$result = $conn->query($sql);
 
if ($result->num_rows >0) {
 
 
    while ($row = mysqli_fetch_assoc($result)) {
        $resArray[] = array_map('utf8_encode', $row);
    }
 }
 
 else {
 echo "No Results Found.";
}
 echo json_encode($resArray);
$conn->close();
?>