| Class 1                   | Class 2            | Relationship |
|---------------------------|--------------------|--------------|
| FileCabinetService        | IRecordValidator   | has-a        |
| FileCabinetDefaultService | FileCabinetService | is-a         |
| FileCabinetCustomService  | FileCabinetService | is-a         |
| FileCabinetDefaultService | DefaultValidator   | has-a        |
| FileCabinetCustomService  | CustomValidator    | has-a        |
| DefaultValidator          | IRecordValidator   | is-a         |
| CustomValidator           | IRecordValidator   | is-a         |