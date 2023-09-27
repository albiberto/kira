# Kira

## Kira User Secrets Setup

Follow these steps to add User Secrets for Kira in your ASP.NET Core project.

### Step 1: Install the Secret Manager tool (if not already installed)

To begin, make sure you have the Secret Manager tool installed globally using the following command:

```bash
dotnet tool install --global dotnet-user-secrets
```

Step 2: Initialize User Secrets

Open your terminal and navigate to your project directory. Then, initialize User Secrets by running:

```bash
dotnet user-secrets init
```

Step 3: Add Kira Secrets

Add the required secrets for the Kira section using the following commands. Replace the placeholders with your actual information:

```bash
dotnet user-secrets set "Kira:Auth:Username" "YourUsername"
dotnet user-secrets set "Kira:Auth:Password" "YourPassword"
dotnet user-secrets set "Kira:Jira:BaseAddress" "JiraAddress"
dotnet user-secrets set "Kira:Jira:Projects:0:Project" "ProjectName"

# Add components if necessary
dotnet user-secrets set "Kira:Jira:Projects:0:IncludedComponents:0" "Component1"
dotnet user-secrets set "Kira:Jira:Projects:0:ExcludedComponents:0" "Component2"

# Add included types if necessary
dotnet user-secrets set "Kira:Jira:Projects:0:IncludedTypes:0:Type" "Type1"
dotnet user-secrets set "Kira:Jira:Projects:0:IncludedTypes:0:IncludedStatus:0" "Status1"
dotnet user-secrets set "Kira:Jira:Projects:0:IncludedTypes:0:ExcludedStatus:0" "Status2"

# Add excluded types if necessary 
dotnet user-secrets set "Kira:Jira:Projects:0:ExcludedTypes:0:Type" "Type2"
dotnet user-secrets set "Kira:Jira:Projects:0:ExcludedTypes:0:IncludedStatus:0" "Status3"
dotnet user-secrets set "Kira:Jira:Projects:0:ExcludedTypes:0:ExcludedStatus:0" "Status4"
```

Step 4: Verify Secrets

To confirm that your secrets have been added successfully, run the following command:

```bash
dotnet user-secrets list
```

Make sure to keep your User Secrets secure and private.

That's it! You've now set up User Secrets for the Kira section in your ASP.NET Core project.