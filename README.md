# Kira

## Kira User Secrets Setup

Follow these steps to add User Secrets for Kira in your ASP.NET Core project.

### Step 1: Install the Secret Manager tool (if not already installed)

To begin, make sure you have the Secret Manager tool installed globally using the following command:

```bash
dotnet tool install --global dotnet-user-secrets
```

### Step 2: Initialize User Secrets

Open your terminal and navigate to your project directory. Then, initialize User Secrets by running:

```bash
dotnet user-secrets init
```

### Step 3: Add Kira Secrets

Add the required secrets for the Kira section using the following commands. Replace the placeholders with your actual information:

```bash
dotnet user-secrets set "Kira:Auth:Username" "Your Mail"
dotnet user-secrets set "Kira:Auth:Password" "Your Password"
dotnet user-secrets set "Kira:Jira:BaseAddress" "Your JIRA API base address"
dotnet user-secrets set "Kira:Jira:Defaults:Projects:0" "Your Target Projects ID"
dotnet user-secrets set "Kira:Jira:Defaults:IncludedComponents:0" "Component1_ID"
dotnet user-secrets set "Kira:Jira:Defaults:ExcludedComponents:0" "Component2_ID"
dotnet user-secrets set "Kira:Jira:Defaults:IncludedTypes:0" "Type1_ID"
dotnet user-secrets set "Kira:Jira:Defaults:ExcludedTypes:0" "Type2_ID"
dotnet user-secrets set "Kira:Jira:Defaults:IncludedStatues:0" "Status1_ID"
dotnet user-secrets set "Kira:Jira:Defaults:ExcludedStatues:0" "Status2_ID"
dotnet user-secrets set "Kira:Jira:Defaults:ExcludedStatues:1" "Status3_ID"
```

Step 4: Verify Secrets

To confirm that your secrets have been added successfully, run the following command:

```bash
dotnet user-secrets list
```

Make sure to keep your User Secrets secure and private.

That's it! You've now set up User Secrets for the Kira section in your ASP.NET Core project.