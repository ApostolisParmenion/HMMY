<html>
<head>
<title>Index</title>
<link rel="stylesheet" href = "Style.css"/>
<meta name="viewport" content="width=device-width, initial-scale=1">
</head>
<body>
<h2>Welcome</h2>

<button class="open-button" onclick="openLogin();closeRegister();">Login</button><br>
<button class="open-button" onclick="openRegister();closeLogin();">Register</button><br>

<div class="formlog-popup" id="login">
  <form action="Login"  method = "post">
  
  
    <input type="text"  name="email" autocomplete="off" required>
    <label for="email" class = "label-email">
    	<span class="content-email"><b>Email</b></span>
    </label>
    
    
    <input type="password"name="password" required>
    <label for="password" class = "label-password">
    	<span class="content-password"><b>Password</b></span>
    </label>
    
    
    <button type="submit" class="btn">Login</button>
    <button type="button" class="btn cancel" onclick="closeLogin()">Close</button>
  </form>
</div>

<div class="formreg-popup" id="register">
<form action="Register"  method = "get">
<input type="text" placeholder="Enter name" name="name" autocomplete="off" required>
<input type="text" placeholder="Enter Lastname" name="lastname" autocomplete="off" required>
<input type="text" placeholder="Enter Email" name="email" autocomplete="off" required>
<input type="text" placeholder="Enter City" name="city" autocomplete="off" required>
<input type="text" placeholder="Enter Country" name="country" autocomplete="off" required>
<input type="text" placeholder="Enter Address" name="address" autocomplete="off" required>
<input type="text" placeholder="Enter ZIPCODE" name="zipcode" autocomplete="off" required>
<input type="text" placeholder="Enter Phone Number" name="phonenumber" autocomplete="off" required>
<input type="password" placeholder="Enter Password" name="password" autocomplete="off" required>
<br>
<button type="submit" class="btn">Register</button>
<button type="button" class="btn cancel" onclick="closeRegister()">Close</button>
</form>
</div>

<script>
function openLogin() {
  document.getElementById("login").style.display = "block";
}
function closeLogin() {
  document.getElementById("login").style.display = "none";
}
function openRegister() {
	  document.getElementById("register").style.display = "block";
	}
	function closeRegister() {
	  document.getElementById("register").style.display = "none";
	}
</script>

</body>
</html>
