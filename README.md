
#  1. Introduction
TASM enables Microsoft Cloud Solution Provider (CSP) Partners to rapidly estimate the cost of hosting VMs in Azure through the automated mapping of virtual machine specifications to equivalent Azure VMs. After mapping source VMs to Azure VMs based on their specification, TASM also retrieves current CSP pricing for each VM.

# 2. Background
Many CSP Partners are seeking to provide services to migrate existing IaaS workloads to Azure. Partners focussing on migration frequently need to estimate the costs of hosting existing VMs on Azure. This tool enables CSP Partners to rapidly estimate the costs for hosting VMs in Azure.

# 3. Benefits
* Rapid high-fidelity, override-enabled mapping of on-premises VMs to Azure VMs
* No installation of agents or runtime data collection required
* Extensible, open-source solution to enable re-use and further development by partners

# 4. Build
You can refer to the [Build guide][1] for generating the executable from the source code.

# 5. Configuration
This tool requires some initial configuration prior to first use. Follow the [Step by step guide for configuration][2].

# 6. User guide
Please refer to the [User guide][3] for more information on how to use the tool.

# 7. Limitations
* This tool and the source code is provided as a sample and no support is provided. 
* The cost of software/workloads running in the VMs is not estimated.
* The operating system for VMs can be mapped to Windows or Linux only. Costs for other operating systems options (e.g. Red Hat Enterprise Linux) are not estimated by the tool.
* Network related costs are also not included and not estimated.

# 8. Contributions
Please refer to the [Contributing.md][4]

# 9. License
Please refer to the [License][5]

[1]: docs/Build.md
[2]: docs/Configure.md
[3]: docs/UserGuide.md
[4]: Contributing.md
[5]: LICENSE
