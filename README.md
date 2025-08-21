StreamLogix-Simulation

StreamLogix-Simulation is a microservices-based simulation platform designed to model and manage logistics operations. Built with .NET Core and Docker, it provides a scalable and modular architecture for simulating various logistics processes.

🚀 Features
	•	Microservices Architecture: Comprises multiple services such as AssetService, AuthService, Client, CompanyService, MovementService, NotificationService, ShipmentService, and SimulationService.
	•	Database Integration: Utilizes a dedicated database project (StreamLogixDBProject) for data management.
	•	Docker Support: Includes a docker-compose.yml file for easy containerization and deployment.
	•	Service Communication: Facilitates inter-service communication for cohesive simulation workflows.

🛠️ Technologies Used
	•	Backend: .NET Core
	•	Containerization: Docker
	•	Database: SQL Server (assumed based on project structure)

📁 Project Structure

StreamLogix-Simulation/
├── AssetService/
├── AuthService/
├── Client/
├── CompanyService/
├── MovementService/
├── NotificationService/
├── ShipmentService/
├── SimulationService/
├── StreamLogixDBProject/
├── docker-compose.yml
└── .gitignore

⚙️ Setup Instructions

Prerequisites
	•	Docker
	•	Docker Compose

Steps
	1.	Clone the Repository

git clone https://github.com/FasttrackNishant/StreamLogix-Simulation.git
cd StreamLogix-Simulation


	2.	Build and Run Containers

docker-compose up --build

This command will build and start all the services defined in the docker-compose.yml file.

	3.	Accessing the Services
	•	Services will be available on their respective ports as defined in the docker-compose.yml file.
	•	Ensure that the services are properly configured to communicate with each other.

🧪 Running Simulations
	•	Once the services are up and running, you can initiate simulations through the SimulationService.
	•	Ensure that all required data and configurations are in place for accurate simulation results.

📄 License

This project is licensed under the MIT License.
