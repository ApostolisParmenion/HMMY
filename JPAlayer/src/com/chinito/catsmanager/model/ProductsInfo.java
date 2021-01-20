package com.chinito.catsmanager.model;


public class ProductsInfo {
    private String name,color,description;
    private Integer barcode;
    public ProductsInfo( String name,Integer barcode, String color, String description) {
        super();
        this.name = name;
        this.barcode = barcode;
        this.color = color;
        this.description = description;
    }
    public String getname() {
        return name;
    }

   
    
    public Integer getbarcode() {
        return barcode;
    }

 
    public String getcolor() {
        return color;
    }
 
    public String getdescription() {
        return description;
    }
 
    
    
}