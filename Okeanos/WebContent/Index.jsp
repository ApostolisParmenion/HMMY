<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="ISO-8859-1">
<title>Products</title>
</head>
<link rel="stylesheet" href = "Style.css"/>
<body>
<h2>Welcome</h2>
<button class="open-button" onclick="openRegister();closeLogin();">Register a Product</button><br>



<div class="formreg-popup" id="register">
	<form action="Register"  method = "get">
		<input type="text" placeholder="Enter Name" name="name" autocomplete="off" required>
		<input type="text" placeholder="Enter Barcode" name="barcode" autocomplete="off" required>
		<input type="text" placeholder="Enter Color" name="color" autocomplete="off" required>
		<input type="text" placeholder="Enter Description" name="description" autocomplete="off" required>
		<br>
		<button type="submit" class="btn">Register</button>
		<button type="button" class="btn cancel" onclick="closeRegister()">Close</button>
	</form>
</div>

<div class="ShowProds" id="ShowProducts">
	<form action="ShowProducts"  method = "get">
		<button type="submit" class="btn">Show Registered Products</button>
	</form>
</div>	
<script>
function openRegister() {
	  document.getElementById("register").style.display = "block";
	}
	function closeRegister() {
	  document.getElementById("register").style.display = "none";
	}
</script>
</body>
</html>