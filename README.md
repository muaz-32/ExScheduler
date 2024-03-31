# ExScheduler
An exam routine generator using AI to satisfy constraints and maximize the output value

## Exam Routine Generator Application

This application addresses the challenge of creating optimal exam schedules. It takes various constraints (e.g., exam duration, student availability, break times) into account and generates a routine that maximizes a user-defined objective function (e.g., minimizing exam conflicts, maximizing time between exams for specific subjects).

## Software Requirements Specification (SRS)

A comprehensive SRS was developed to clearly define project functionalities, user requirements, and system specifications. This document ensured all stakeholders were aligned on project goals and delivered a clear roadmap for development.

# Features
## Zinc Modeling & Problem Solving

The real-world exam scheduling scenario was modeled using the Zinc constraint modeling language. This involved defining constraints that represent real-life limitations (e.g., exam duration, student availability), designing a utility function to express the optimization goals (e.g., minimizing conflicts), and selecting suitable data structures for efficient problem solving using Constraint Satisfaction Programming (CSP) and Artificial Intelligence (AI) solvers.

## Minizinc Implementation & Troubleshooting

Minizinc was chosen to translate the Zinc model into a format compatible with various CSP/AI solvers. During this process, technical hurdles were encountered due to limitations in Minizinc documentation and syntax errors. These challenges were overcome through independent research and troubleshooting, fostering valuable skills in problem-solving and software adaptation.

## ASP.NET Backend Development

The Minizinc model's output was seamlessly integrated with an ASP.NET backend. This involved mapping the generated exam schedule data and utilizing it within the backend functionalities. Additionally, several well-structured APIs were implemented in ASP.NET to facilitate communication between the application's different components.

## Frontend & JS Integration

A user-friendly frontend was developed and merged with the ASP.NET backend. JavaScript (JS) code was written to handle user interactions with the application, ensuring a smooth and responsive user experience.
