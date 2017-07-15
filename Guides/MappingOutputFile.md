#  1. Introduction
This document provides information about the columns in the Mapping (Output) file.


#  2. Columns in Mapping (Output) file

All the input columns from the VM Specifications Input file are also written to the output file. 

The following are the additional columns that are added in the output:

* **Is Valid?** - *Yes* or *No*, indicating the status of validation for the input columns. E.g. *Yes*
* **Validation Message** - Validation message that contains information about columns that failed validation. E.g. *Validation Failed:'CPU Cores' should be a positive integer*
* **Mapped Azure VM SKU** - Azure VM SKU mapped by the tool as per the input specifications provided. The algorithm used by the tool is to take the Azure VM SKU that matches the input specifications and has the lowest cost as per the rate card fetched. By default, there is no downgrade in the number of cores and memory when mapping to an Azure SKU. However, the algorithm can be fine-tuned to a certain extent by modifying the mapping co-efficients in the options of the tool. If an override is provided and if it is valid in the region selected, the algorithm is not used and the instance is mapped to an Azure VM SKU as per Override value specified. This value will be blank for a row where **Is valid?** is *No*. E.g. *Standard\_A4m\_v2*
* **Azure VM Cores** - Number of cores of the mapped Azure VM SKU E.g. *4*
* **Azure VM Memory** - Memory of the mapped Azure VM SKU. The units (In GB or In MB) will be added to the header and will be same as the units for the input values. The units can be changed before running the mapping process in the options of the tool. E.g. *32*
* **Compute Hours Monthly Cost** - Monthly Cost Estimate for the compute hours component of the mapped Azure VM SKU as per the rate card fetched. The units (e.g. *USD*) will be added to the header. Eg. *250.50* 
* **Premium Disks Monthly Cost** - Monthly Cost Estimate for the mapped premium managed disks. This will be 0 if no premium managed disks were mapped. The units (e.g. *USD*) will be added to the header. E.g. *19.71*
* **Standard Disks Monthly Cost** - Monthly Cost Estimate for the mapped standard managed disks. This will be 0 if no standard managed disks were mapped. Please note that this is only the storage capacity costs associated with the standard disks mapped and does not include storage transactions costs. The units (e.g. *USD*) will be added to the header. E.g. *2.94*
* **Azure VM Total Monthly Cost** - Monthly Cost Estimate for the mapped Azure VM SKU with storage. This will be sum of: **Compute Hours Monthly Cost**, **Premium Disks Monthly Cost** and **Standard Disks Monthly Cost**. The units (e.g. *USD*) will be added to the header. E.g. *2.94*
* **Monthly Gross Margin Estimates** - Estimates of Gross margin for the instances based upon mapped Azure VM cost and current pricing provided in the input. This value is obtained by subtracting **Azure VM Total Monthly Cost** from **PricePerMonth**. The units (e.g. *USD*) will be added to the header. E.g. *125.80*
* **Premium Disks** - Premium Managed Disks mapped by the tool. This will be blank if no premium managed disks were mapped. E.g. *4 x P30; 1 x P4*
* **Standard Disks** - Standard Managed Disks mapped by the tool. This will be blank if no standard managed disks were mapped. E.g. *2 x S30; 1 x S4*
* **Compute Hours Rate** - The rate per hour for the compute hours component for the mapped Azure VM SKU. The units (e.g. *USD*) will be added to the header. E.g. *0.45*

