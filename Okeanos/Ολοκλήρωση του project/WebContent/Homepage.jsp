<%@ page language="java" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<html>
<head>
<title>Index</title>
<link rel="stylesheet" href = "Style.css"/>
<meta name="viewport" content="width=device-width, initial-scale=1">
</head>

<body>
<% if(request.getParameter("selfEmail") == null){
	response.sendRedirect("Index.jsp");
	}%>

<div class="MyProfile">
	<form action="Showprofile"  method = "get">
		<input type="hidden" value="${selfEmail}" name = "selfEmail"></input>
		<button type="submit" class="button1"><h1>Show Profile</h1></button>
	</form>
</div>
<br>


<div class="searchform" >
	<form action="SearchUsers"  method = "get">
	 
		<input type="text"  name="NamLas" required></input>
		<label for="NamLas" class = "label-NamLas">
	    	<span class="content-NamLas"><b>Search for a User</b></span>
	    </label>
	    
		<input type = "hidden" value="${selfEmail}" name = "selfEmail"></input>
		<button type="submit" class="button2"><b>Search!</b></button>
	</form>
</div>



<div class="Addform" >
	<c:if test = "${success==0}">
	<script>
  		alert("Email Not Found!");
	</script>
</c:if>
<c:if test = "${success==1}">
	<script>
  		alert("Successfully added!");
	</script>
</c:if>
	<form action="AddPresent"  method = "get">
		<input type="text"  name="present" required>
		<label for="present" class = "label-present">
			<span class="content-present"><b>Add Present to Profile</b></span>
	    </label>
	    <input type="number"  name="amount" required>
		<input type = "hidden" value="${selfEmail}" name = "selfEmail"></input>
		<button type="submit" class="button3">Add!</button>
	</form>
</div>	
	
<div class="Logout">
	<form action="Index.jsp"  method = "get">
		<button type = "submit" class ="button2"><h1>Logout</h1></button>
	</form>
</div>
</body>
</html>


