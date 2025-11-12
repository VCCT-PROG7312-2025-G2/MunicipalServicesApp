# MunicipalServicesApp
📋 Project Overview
This municipal services platform enables:
•	Service Request Management: Residents report issues, employees track resolution
•	Event Coordination: Community event discovery and registration
•	Notification System: Real-time alerts for service updates
•	Dashboard Analytics: Performance metrics and reporting

🏗️ Architecture
Technology Stack
•	Backend: ASP.NET Core MVC
•	Frontend: Bootstrap 5, jQuery
•	Data Structures: Custom collections (Hash Table, Stack, Set, Priority Queue)
•	Storage: Session-based with JSON serialization

🔄 Changelog
Part 3 Improvements
•	Improved Error Handling and user feedback
Part 2 Improvements
•	Added Authentication System with role-based access control
•	Enhanced Data Structures with proper serialization support
•	Added File Upload capability for service requests
•	Implemented Session Management for user data persistence
•	Custom Data Structures implementation
•	Advanced Event Search with multiple filters
•	Recommendation Engine based on user behavior
•	Priority Queue for event management
Part 1 Foundation
•	Basic MVC Architecture with controllers and views
•	Service Request System for issue reporting
•	Event Management with categories and filtering
•	Notification System for user updates
•	Responsive Design with Bootstrap integration

🎯 Features
For Residents
•	Report municipal issues with photo attachments
•	Browse and register for community events
•	Receive service update notifications
•	Track request resolution progress
For Employees
•	Manage service request workflow
•	Update request status and assignments
•	Coordinate community events
•	Access performance dashboards
For Administrators
•	Full system oversight
•	User management capabilities
•	Analytics and reporting
•	System configuration

📊 Data Structure Explanation
Custom Data Structures Implementation
1. CustomDictionary<TKey, TValue> (Hash Table)
Purpose: Efficient key-value storage for event categorization and user preferences
Implementation:
•	Chaining collision resolution
•	Dynamic resizing (load factor > 0.75)
•	O(1) average case for insert/lookup
User Benefits:
•	Fast event categorization and filtering
•	Quick user preference retrieval
•	Efficient category management
Municipal Benefits:
•	Scalable handling of large event datasets
•	Rapid search and retrieval operations
•	Memory-efficient storage
2. CustomStack<T> (LIFO Collection)
Purpose: Manage service request updates and media attachments in chronological order
Implementation:
•	Linked list-based stack
•	Serialization support for session storage
•	LIFO operations (Push, Pop, Peek)
User Benefits:
•	Clear audit trail of request updates
•	Sequential media attachment management
•	Natural workflow for status updates
Municipal Benefits:
•	Maintains complete request history
•	Efficient memory usage for linear data
•	Simple undo/redo capabilities for workflows
3. EventSet<T> (Set Collection)
Purpose: Handle unique event categories, dates, and search results
Implementation:
•	Built on CustomDictionary for O(1) operations
•	Set operations (Union, Intersection)
•	Duplicate prevention
User Benefits:
•	Unique category listings
•	Efficient search result combination
•	No duplicate events in recommendations
Municipal Benefits:
•	Ensures data integrity
•	Efficient filtering operations
•	Scalable for large datasets
4. EventPriorityQueue (Heap-based Queue)
Purpose: Prioritize events by date and featured status
Implementation:
•	Min-heap for chronological ordering
•	Custom comparison (date → featured status → title)
•	Efficient enqueue/dequeue operations
User Benefits:
•	Chronological event display
•	Featured event prioritization
•	Efficient upcoming events retrieval
Municipal Benefits:
•	Optimal event scheduling
•	Important event highlighting
•	Efficient batch processing
Real-World Impact
For Residents:
•	Faster Search Results: Hash tables enable quick event filtering
•	Better Recommendations: Sets ensure diverse, relevant suggestions
•	Clear Communication: Stacks maintain coherent request histories
For Municipal Staff:
•	Efficient Workflow: Priority queues highlight urgent matters
•	Scalable Management: Custom collections handle growing data volumes
•	Reliable Performance: Optimized algorithms ensure responsive system
Technical Advantages:
•	Memory Efficiency: Custom implementations reduce overhead
•	Performance: Optimized for specific use cases
•	Maintainability: Clean, documented code structure
•	Extensibility: Easy to modify for new requirements

 How to Use the Application
1. Login
Open the application in your browser
Use any of the demo accounts above to log in
You'll be redirected to the Dashboard after successful login

2. Dashboard Features
Service Requests Overview: View total, resolved, and pending requests
Notifications: See recent system notifications
Quick Actions: Access common features quickly
User Info: See your role and name in the top right

3. Service Requests
View Requests: Click "Service Requests" in sidebar to see all requests
Report Issues: Use "Report New Issue" to submit service requests
Track Status: Monitor request progress with status updates

4. Events Management
Browse Events: Click "Local Events" to see community events
Search & Filter: Find events by category, date, or keywords
Event Details: View full event information and registration status

5. Notifications
View Alerts: Check important updates and announcements
Mark as Read: Manage your notification inbox
Categories: Different types (Emergency, Service Updates, General)

🔧 Key Features by User Role
Administrator (admin)
Full access to all system features
Can manage all service requests
System configuration access
User management capabilities

Employee (employee1, employee2)
Access to service request management
Can update request statuses
View assigned tasks and notifications
Event management capabilities
Resident (resident)
Submit service requests
Browse community events
View personal notifications
Basic dashboard access

📱 Navigation
Sidebar Menu: Access all main features
Dashboard: Homepage with overview
Service Requests: Manage issue reports
Local Events: Community events calendar
Notifications: System alerts and updates
Settings: User preferences (if implemented)

🛠️ Troubleshooting
Common Issues:
Build Errors:
Ensure .NET 6.0+ SDK is installed
Run dotnet restore to restore packages
Check for missing dependencies
Runtime Errors:
Clear browser cache
Restart the application
Check terminal for error messages

Login Issues:
Verify username/password combination
Use the exact demo credentials provided
Check browser console for errors
Session Problems:
Application uses session storage
Data persists during browser session
Logout/login to refresh session data

Development Notes:
Data is stored in browser session (resets on browser close)
File uploads are saved to wwwroot/uploads directory
Custom collections used for data structures
Bootstrap 5 for responsive UI design

📞 Support
If you encounter issues:
Check the terminal for error messages
Verify all prerequisites are installed
Try different browsers if issues persist

🚀 How to Compile and Run
Method 1: Using Visual Studio
Open the project:
Launch Visual Studio
Select "Open a project or solution"
Navigate to your project folder and select the .csproj file

Build the project:
Press Ctrl+Shift+B or go to Build → Build Solution
Run the application:
Press F5 or Ctrl+F5 to run
The app will open in your default browser at https://localhost:7000 or http://localhost:5000

Youtube:https://youtu.be/JX9WlS8X46Y 

Reference list
GeeksforGeeks (2022). Events and Event Handlers in C#. [online] GeeksforGeeks. Available at: https://www.geeksforgeeks.org/c-sharp/events-and-event-handlers-in-c-sharp/ [Accessed 15 Oct. 2025].
GeeksforGeeks (2024). How to Serialize JSON in JavaScript ? [online] GeeksforGeeks. Available at: https://www.geeksforgeeks.org/javascript/how-to-serialize-json-in-javascript/ [Accessed 15 Oct. 2025].
tutorials-EU (2025). Using JSON IN C#! Serialization & Deserialization made easy! [online] Youtu.be. Available at: https://youtu.be/w6M-Bj-tfv4?si=-EDJLPgSzcdcLaic [Accessed 15 Oct. 2025].
W3schools (2025). C# Exceptions (Try..Catch). [online] www.w3schools.com. Available at: https://www.w3schools.com/cs/cs_exceptions.php [Accessed 15 Oct. 2025].


