EmployeeController is an API that performs CRUD operations for employee management. The API provides two different versions for data retrieval using both [FromBody] and [FromQuery]

API Endpoints

**Get All Employees**
- **Operation:** **Get all employees**
- **HTTP Method:** `GET`
- **API Endpoint:** `/api/employees/all`

**Get Employee by ID**
- **Operation:** **Get employee by ID**
- **HTTP Method:** `GET`
- **API Endpoints:**  
  - **Query:** `/api/employees/query-by-id?id={id}`
  - **Route:** `/api/employees/route/{id}`

**Create Employee**
- **Operation:** **Create employee**
- **HTTP Method:** `POST`
- **API Endpoints:**  
  - **Query:** `/api/employees/query?name=...&email=...`
  - **Body:** `/api/employees/body`

**Update Employee**
- **Operation:** **Update employee**
- **HTTP Method:** `PUT`
- **API Endpoints:**  
  - **Query:** `/api/employees/query?id=...&name=...`
  - **Body:** `/api/employees/body?id=...`

**Partial Update Employee**
- **Operation:** **Partial update employee**
- **HTTP Method:** `PATCH`
- **API Endpoints:**  
  - **Query:** `/api/employees/query?id=...&name=...`
  - **Body:** `/api/employees/body?id=...`

**Filter and Sort Employees**
- **Operation:** **Filter & sort employees**
- **HTTP Method:** `GET`
- **API Endpoint:** `/api/employees/list-filtered?name=...&sortField=...&sortOrder=asc/desc`

