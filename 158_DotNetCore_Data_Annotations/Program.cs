//LengthAttribute

[Length(2, 20)]
public string Name { get; set; }

POST {{DotNet8NewDataAnnotations_HostAddress}}/products
Content-Type: application/json

{
  "name": "a",
  "price": 1000,
  "category": "Computers",
  "subCategory": "Laptops",
  "description": "UHJvZHVjdCBkZXNjcmlwdGlvbg=="
}

POST {{DotNet8NewDataAnnotations_HostAddress}}/products
Content-Type: application/json

{
  "name": "aaaaaaaaaaaaaaaaaaaaa",
  "price": 1000,
  "category": "Computers",
  "subCategory": "Laptops",
  "description": "UHJvZHVjdCBkZXNjcmlwdGlvbg=="
}

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": [
      "The field Name must be a string or collection type with a minimum length of '2' and maximum length of '20'."
    ]
  },
  "traceId": "00-58c98391eb6f9120395914371234dca5-687e31c0e69b5339-00"
}

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": [
      "The field Name must be a string or collection type with a minimum length of '2' and maximum length of '20'."
    ]
  },
  "traceId": "00-feced6c3c3be0a1d80246f316ec87e59-2fef27b78e9443e5-00"
}


//MinimumIsExclusive & MaximumIsExclusive

[Range(1, 100, MinimumIsExclusive = true, MaximumIsExclusive = false)]
public double Price { get; set; }

POST {{DotNet8NewDataAnnotations_HostAddress}}/products
Content-Type: application/json

{
  "name": "Surface Pro",
  "price": 1,
  "category": "Computers",
  "subCategory": "Laptops",
  "description": "UHJvZHVjdCBkZXNjcmlwdGlvbg=="
}

POST {{DotNet8NewDataAnnotations_HostAddress}}/products
Content-Type: application/json

{
  "name": "Surface Pro",
  "price": 1001,
  "category": "Computers",
  "subCategory": "Laptops",
  "description": "UHJvZHVjdCBkZXNjcmlwdGlvbg=="
}


{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Price": [
      "The field Price must be between 1 exclusive and 1000."
    ]
  },
  "traceId": "00-de08f4da83c21e5a16055372d2f45464-ab1c5ed37db857b1-00"
}

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Price": [
      "The field Price must be between 1 exclusive and 1000."
    ]
  },
  "traceId": "00-de08f4da83c21e5a16055372d2f45464-5927434ebb6c06af-00"
}

//Base64StringAttribute
[Base64String]
public string Description { get; set; }

POST {{DotNet8NewDataAnnotations_HostAddress}}/products
Content-Type: application/json

{
  "name": "Surface Pro",
  "price": 1000,
  "category": "Computers",
  "subCategory": "Laptops",
  "description": "abc#"
}

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Description": [
      "The Description field is not a valid Base64 encoding."
    ]
  },
  "traceId": "00-de08f4da83c21e5a16055372d2f45464-95bc9914949640a3-00"
}

//AllowedValuesAttribute & DeniedValuesAttribute

[AllowedValues("Computers", "Video Games")]
public string Category { get; set; }

POST {{DotNet8NewDataAnnotations_HostAddress}}/products
Content-Type: application/json

{
  "name": "Surface Pro",
  "price": 1000,
  "category": "Smartwatch",
  "subCategory": "Laptops",
  "description": "UHJvZHVjdCBkZXNjcmlwdGlvbg=="
}

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Category": [
      "The Category field does not equal any of the values specified in AllowedValuesAttribute."
    ]
  },
  "traceId": "00-de08f4da83c21e5a16055372d2f45464-226065e5054bd14b-00"
}

[DeniedValues("Printers")]
public string SubCategory { get; set; }

POST {{DotNet8NewDataAnnotations_HostAddress}}/products
Content-Type: application/json

{
  "name": "Surface Pro",
  "price": 1000,
  "category": "Computers",
  "subCategory": "Printers",
  "description": "UHJvZHVjdCBkZXNjcmlwdGlvbg=="
}

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "SubCategory": [
      "The SubCategory field equals one of the values specified in DeniedValuesAttribute."
    ]
  },
  "traceId": "00-de08f4da83c21e5a16055372d2f45464-c5c49c88ea3291b5-00"
}