# Citrix Kernel

> **Next-Gen Low-Level Core Engine built on the Cosmos Framework**

Citrix Kernel is an independent, 100% free and open-source operating system kernel built on top of the hardware-accelerated **Cosmos** framework. Designed for ultra-low latency, modular expansion, and native process management, it serves as a lightweight foundation for modern low-level system development.

---

## Key Features

* **Cosmos Architecture**: Leverages the Cosmos OS toolkit for low-level hardware control, boot execution, and execution stability.
* **Low Latency Scheduling**: Built-in thread scheduling optimized for minimal I/O delay (< 1ms execution targets).
* **Direct Hardware Interfacing**: Direct control over memory allocation, display outputs, and peripheral input pipelines.
* **Fully Open-Source**: Free to clone, build, modify, and distribute without license restrictions.

---

## Getting Started

### Prerequisites

To build and run Citrix Kernel, ensure you have the following installed:

1. **Visual Studio 2022** (with `.NET Desktop Development` workload)
2. **Cosmos OS User Kit** (latest release)
3. **VMware Workstation Player** or **VirtualBox** (for kernel emulation and testing)

### Installation & Build

1. Clone the repository:
```bash
git clone https://github.com/mufasa-the-dev/citrix.git
cd citrix

```


2. Open the solution file in Visual Studio:
```bash
start Citrix.sln

```


3. Build and launch the kernel:
* Set `Citrix.Boot` (or equivalent boot project) as the Startup Project.
* Press `F5` to compile and launch inside the VMware/VirtualBox environment.



---

## Project Structure

```text
citrix/
├── src/
│   ├── Kernel/          # Core kernel logic, initialization, and entry points
│   ├── Drivers/         # Custom hardware drivers and display interfaces
│   └── System/          # Memory management, utilities, and process scheduler
├── docs/                # Architecture notes and API references
├── Citrix.sln           # Visual Studio Solution
└── README.md

```

---

## Contributing

Contributions are welcome! If you want to add new drivers, optimize kernel routines, or fix issues:

1. Fork the repository.
2. Create your feature branch (`git checkout -b feature/AmazingFeature`).
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`).
4. Push to the branch (`git push origin feature/AmazingFeature`).
5. Open a Pull Request.

---

## License

Distributed under the **MIT License**. See `LICENSE` for more details.

---

Would you like to add specific build commands, custom API docs, or hardware requirements to this README?
