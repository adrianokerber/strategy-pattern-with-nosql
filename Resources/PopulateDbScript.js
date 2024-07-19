// 1. Create manually the Database named `JobAllocationDB`
// 2. Run the code below on your database via CLI or via Studio3T for example... 

// Create Policies collection
db.getCollection("Policies").insertMany([
    {
        "_t": "JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.PolicyCapSalary, JobAllocation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
        "Data": {
            "maximumSalary": 30000
        }
    },
    {
        "_t": "JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.PolicyDisabilities, JobAllocation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
        "Data": {
            "disabilities": [
                {
                    "code": 5,
                    "name": "AssistedDisabled"
                },
                {
                    "code": 4,
                    "name": "VisualAmplified"
                },
                {
                    "code": 2,
                    "name": "VisualBraille"
                },
                {
                    "code": 3,
                    "name": "Intellectual"
                },
                {
                    "code": 1,
                    "name": "Curatorship"
                }
            ]
        }
    },
    {
        "_t": "JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.PolicyMaxAgePerState, JobAllocation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
        "Data": {
            "states": [
                {
                    "federatedState": "PB",
                    "maximumAge": 60
                },
                {
                    "federatedState": "AP",
                    "maximumAge": 60
                },
                {
                    "federatedState": "RR",
                    "maximumAge": 60
                },
                {
                    "federatedState": "TO",
                    "maximumAge": 60
                }
            ]
        }
    },
    {
        "_t": "JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.PolicyDegreeLevel, JobAllocation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
        "Data": {
            "degreeCode": 7
        }
    }
]);

// Create Companies collection
db.getCollection("Companies").insertMany([
    {
        "code": "1",
        "name": "Google",
        "allocationTypePreference": {
            "code": 1,
            "description": "REMOTE"
        }
    },
    {
        "code": "2",
        "name": "Meta",
        "allocationTypePreference": {
            "code": 1,
            "description": "REMOTE"
        }
    },
    {
        "code": "3",
        "name": "Amazon",
        "allocationTypePreference": {
            "code": 2,
            "description": "HYBRID"
        }
    },
    {
        "code": "4",
        "name": "nVidia",
        "allocationTypePreference": {
            "code": 2,
            "description": "HYBRID"
        }
    }
]);