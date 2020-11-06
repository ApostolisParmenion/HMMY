package com;


public class ProductsInfo {
    private String name,color,description;
    private Integer id,barcode;
    public ProductsInfo(Integer id, String name,Integer barcode, String color, String description) {
        super();
        this.id=id;
        this.name = name;
        this.barcode = barcode;
        this.color = color;
        this.description = description;
    }
    public String getname() {
        return name;
    }
    public void setname(String name) {
        this.name = name;
    }
   
    
    public Integer getbarcode() {
        return barcode;
    }
    public void setbarcode(Integer barcode) {
        this.barcode = barcode;
    }
   
 
    public String getcolor() {
        return color;
    }
    public void setcolor(String color) {
        this.color = color;
    }

    public String getdescription() {
        return description;
    }
    public void setdescription(String description) {
        this.description = description;
    }
    
    
    public Integer getId() {
        return id;
    }
    public void setId(Integer Id) {
    	this.id=Id;
    }
 
    
}