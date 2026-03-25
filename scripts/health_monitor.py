import requests
from colorama import Fore, Style, init
from datetime import datetime

init(autoreset=True)

SERVICES = [
    {
        "name": "Auth Service",
        "url": "http://localhost:5002/api/Auth/health",
        "port": 5002
    },
    {
        "name": "Property Service",
        "url": "http://localhost:5001/api/Properties/health",
        "port": 5001
    },
    {
        "name": "Analytics Service",
        "url": "http://localhost:5003/api/Analytics/health",
        "port": 5003
    }
]

def check_service(service):
    try:
        response = requests.get(service["url"], timeout=5)
        if response.status_code == 200:
            data = response.json()
            print(Fore.GREEN + f"  ✅ {service['name']:<25} "
                  f"PORT {service['port']}  "
                  f"Status: {data.get('status', 'OK')}")
            return True
        else:
            print(Fore.RED + f"  ❌ {service['name']:<25} "
                  f"PORT {service['port']}  "
                  f"HTTP {response.status_code}")
            return False
    except requests.ConnectionError:
        print(Fore.RED + f"  ❌ {service['name']:<25} "
              f"PORT {service['port']}  "
              f"NOT RUNNING")
        return False
    except requests.Timeout:
        print(Fore.YELLOW + f"  ⚠️  {service['name']:<25} "
              f"PORT {service['port']}  "
              f"TIMEOUT")
        return False

def main():
    print(Fore.CYAN + "\n" + "="*55)
    print(Fore.CYAN + "   🏠 PropNest — API Health Monitor")
    print(Fore.CYAN + f"   Checked at: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    print(Fore.CYAN + "="*55 + "\n")

    results = []
    for service in SERVICES:
        result = check_service(service)
        results.append(result)

    total = len(results)
    passed = sum(results)
    failed = total - passed

    print(Fore.CYAN + "\n" + "-"*55)

    if failed == 0:
        print(Fore.GREEN + f"\n  🎉 All {total} services are healthy!\n")
    else:
        print(Fore.RED + f"\n  ⚠️  {passed}/{total} services running. "
              f"{failed} service(s) down!\n")
        print(Fore.YELLOW + "  Fix: Run 'dotnet run' in each service folder\n")

    print(Fore.CYAN + "="*55 + "\n")
    return failed == 0

if __name__ == "__main__":
    main()