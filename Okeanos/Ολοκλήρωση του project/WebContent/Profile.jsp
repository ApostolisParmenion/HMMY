<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<!DOCTYPE html>
<html>
<style>
body {
  background-image: url('photos/wel.jpg');
  background-repeat: no-repeat;
  background-attachment: fixed;
  background-size: cover;
}
table{
border-collapse:collapse;
}
th, td {
  text-align: center;
  padding: 8px;
}
tr:hover {background-color:#064564;}
tr:nth-child(even) {background-color: #0a6b9a;}
</style>
<head>
<meta charset="ISO-8859-1">
<title>Profile</title>
</head>

<body>
<% if(request.getParameter("selfEmail") == null){
	response.sendRedirect("Index.jsp");
	}%>
<table>
  <tr>
    <th>Name</th>
    <th>Lastname</th>
    <th>Country</th>
    <th>City</th>
    <th>Address</th>
    <th>Zipcode</th>
    <th>Phone</th>
    <th>Email</th>
  </tr>
  <tr>
    <td>${name}</td>
    <td>${lastname}</td>
    <td>${country}</td>
    <td>${city}</td>
    <td>${address}</td>
    <td>${zipcode}</td>
    <td>${phone}</td>
    <td>${email}</td>
  </tr>
</table>
<br><br><br>
	<table style="border:2px solid black;" >
	  <tr>
	    <th>Presents</th>
	    <c:choose>
		    <c:when test = "${myemail == false}">
	    		<th>Amount Left</th>
	    		<th>Option1</th>
	    		<th>Option2</th>
	    	</c:when>
	    	<c:otherwise>
	    		<th>Option</th>
	    	</c:otherwise>
	    </c:choose>
	  </tr>

	  <c:forEach items="${listPresents}" var="present" varStatus="status">
	        <tr style="border:2px solid black;">
	    		<th>${present}
	    		<c:if test = "${listChecked[status.index]==1 && myemail==false}">
	    			(Already Selected)
	    		</c:if>
	    		</th>
	    		
		    		<c:if test = "${listChecked[status.index]==0 && myemail==false}">
		    			<th>${listAmounts[status.index]}</th>
		    		</c:if>
		    		
	    		
	    		<th>
	    		<c:choose>
		    		<c:when  test = "${myemail}" >
		    			<form action = "Showprofile">
			    			<input type="hidden" value="${selfEmail}" name = "selfEmail"></input>
			    			<input type="hidden" value="${email}" name = "email"></input>
			    			<input type="hidden" value="${present}" name = "presentname"></input>
			    			<input type="hidden" value="1" name = "delete"></input>
			    			<button type="submit">Delete</button>
		    			</form>
		    		</c:when>
		    		
		    		<c:otherwise>
		    			<c:if test = "${listChecked[status.index]==0}">
			    			<form action = "Showprofile">
				    			<input type="hidden" value="${selfEmail}" name = "selfEmail"></input>
				   				<input type="hidden" value="${email}" name = "email"></input>
				   				<input type="hidden" value="${present}" name = "presentname"></input>
				   				<input type="hidden" value="0" name = "addAmount"></input>
					   			<input type="hidden" value="1" name = "select"></input>
					   			<button type="submit">Select</button>
				   			</form>
				   		</c:if>
				   		<c:if test = "${selfEmail=='admin1234'}">
				   			<form action = "Showprofile">
					   			<input type="hidden" value="${selfEmail}" name = "selfEmail"></input>
					   			<input type="hidden" value="${email}" name = "email"></input>
					   			<input type="hidden" value="${present}" name = "presentname"></input>
					   			<input type="hidden" value="0" name = "addAmount"></input>
					   			<input type="hidden" value="1" name = "delete"></input>
					   			<button type="submit">Delete</button>
				    		</form>
				    	</c:if>
	         		</c:otherwise>
	         	</c:choose>
	         	</th>
	         	<th>
		         	<c:choose>
			    		<c:when  test = "${ ! myemail}" >
			    			<c:if test = "${listChecked[status.index]==0}">
				    			<form action = "Showprofile">
					    			<input type="hidden" value="${selfEmail}" name = "selfEmail"></input>
					    			<input type="hidden" value="${email}" name = "email"></input>
					    			<input type="hidden" value="${present}" name = "presentname"></input>
					    			<input type="number"  name = "addAmount"></input>
					    			<button type="submit">Add funds</button>
				    			</form>
				    		</c:if>
			    		</c:when>
				   	</c:choose>
	         	</th>	
	    		
	    	</tr>
	    </c:forEach>
	</table>

<form action = "GoHomepage">

<input type="hidden" value="${selfEmail}" name = "selfEmail"></input>
<button type="submit">Go to homepage!</button>
</form>
</body>
</html>