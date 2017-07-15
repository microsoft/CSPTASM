#  1. Introduction
This document provides information about the columns in the Virtual machine specifications (Input) file. You can generate a sample of this file from the tool. 

The file format has to be CSV. 
Each row within the csv file corresponds to a VM (on-premises or competing cloud platform) or a physical server.
The file can contain a header. The text in the header is ignored and tool uses the sequence of columns to map them to fields expected in input file.

#  2. Columns in Virtual machine specifications (Input) file

* **InstanceName** - Name of the Virtual machine, required. E.g. *FabrikamWeb01*
* **OperatingSystem** - Operating System, required. Should be one of the keywords or the text must contain one of the keywords from the Windows keywords list or Linux keywords list. You can view or modify the list of keywords in the options of the tool. E.g. *Windows*
* **CPU Cores** - Number of CPU Cores, required, should be a positive integer. E.g. *2*
* **Memory** - Memory, required, should be a positive number without any unit specifications. Default unit is (*in GB*), this can be changed in options of the tool. Can contain decimal values. E.g. *4.5*
* **SSDStorageInGB** - Total amount of SSD Storage associated with data disks, optional, if provided should be a non-negative number without any unit specifications. Can be *0*, if left blank - assumed as 0. Unit is (*in GB*). Can contain decimal values. E.g. 2048
* **SSDNumOfDisks** - Number of SSD disks associated with data disks, optional, if provided should be a non-negative integer. Can be *0*, if left blank - assumed as 0. If provided, this is used in mapping SSD Storage to premium managed disks. E.g. *3*
* **HDDStorageInGB** - Total amount of HDD Storage associated with data disks, optional, if provided should be a non-negative number without any unit specifications. Can be *0*, if left blank - assumed as 0. Unit is (*in GB*). Can contain decimal values. E.g. *2048*
* **HDDNumOfDisks** - Number of HDD disks associated with data disks, optional, if provided should be a non-negative integer. Can be *0*, if left blank - assumed as 0. If provided, this is used in mapping HDD Storage to standard managed disks. E.g. *3*
PricePerMonth - Current pricing for the instance without any unit specifications, optional, if provided should be non-negative number. Unit should be the same as the currency of the CSP region selected when running the tool.  Can contain decimal values. Though this is optional field, it is recommended to provide a value since, if left blank, it is assumed as 0. As a result, the gross margin estimates in the output file, that is calculated based on **PricePerMonth**, will not be a correct value for this row. E.g. *1200*
* **AzureVMOverride** - Override SKU mapping for the instance, optional, to be specified only if you have already mapped the instance to a specific SKU and do not want the tool to map using its algorithm. If provided should be the text specifying the Azure VM SKU. Can be left blank. E.g. *Standard\_D2\_v2*
* **Comments** - a text/comments that you would like to specify that is also included in the output file, optional, if provided should not exceed 1000 characters. E.g. *This is a VM for hosting the Sales Website*

**Additional notes**: The OS Disk size should *not* be included in the **SSDStorageInGB** and/or **HDDStorageInGB**. Also, the OS Disk should *not* be included in the **SSDNumOfDisks** and/or **HDDNumOfDisks**. The fields: **SSDStorageInGB**, **HDDStorageInGB**, **SSDNumOfDisks** and **HDDNumOfDisks** should include information of data disks only. The OS Disk for each instance is mapped to a managed disk by default and added in the output. The managed disk size that will be mapped for the OS disk can be changed in the options of the tool.

Back to [User guide][1]

[1]: UserGuide.md




