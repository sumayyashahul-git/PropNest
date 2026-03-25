import requests
import json
import random
from faker import Faker

fake = Faker()

AUTH_URL = "http://localhost:5002/api"
PROPERTY_URL = "http://localhost:5001/api"

UAE_COMMUNITIES = [
    "Downtown Dubai", "Dubai Marina", "Palm Jumeirah",
    "Business Bay", "JBR", "Arabian Ranches", "Emirates Hills",
    "Jumeirah Village Circle", "Al Reem Island", "Saadiyat Island",
    "Yas Island", "Al Bateen", "Khalidiyah", "Al Raha Beach",
    "Meydan", "DIFC", "City Walk", "Bluewaters Island"
]

UAE_EMIRATES = ["Dubai", "Abu Dhabi", "Sharjah", "Ajman"]

PROPERTY_TYPES = [0, 1, 2, 3, 4, 5]
TYPE_NAMES = ["Apartment", "Villa", "Townhouse", "Penthouse", "Studio", "Office"]

LISTING_TYPES = [0, 1]

PRICE_RANGES = {
    0: (400000, 5000000),
    1: (800000, 15000000),
    2: (1000000, 8000000),
    3: (2000000, 20000000),
    4: (300000, 2000000),
    5: (500000, 10000000)
}

def get_agent_token():
    print("\n🔐 Logging in as agent...")
    response = requests.post(f"{AUTH_URL}/Auth/login", json={
        "email": "agent@propnest.ae",
        "password": "Test@123"
    })
    if response.status_code == 200:
        token = response.json()["token"]
        print("  ✅ Login successful!")
        return token
    else:
        print("  ❌ Login failed! Make sure agent@propnest.ae exists.")
        return None

def generate_property(prop_type):
    community = random.choice(UAE_COMMUNITIES)
    emirate = "Dubai" if community in [
        "Downtown Dubai", "Dubai Marina", "Palm Jumeirah",
        "Business Bay", "JBR", "Meydan", "DIFC",
        "City Walk", "Bluewaters Island", "Jumeirah Village Circle"
    ] else random.choice(UAE_EMIRATES)

    min_price, max_price = PRICE_RANGES[prop_type]
    price = round(random.randint(min_price, max_price) / 1000) * 1000

    bedrooms = random.choice([0, 1, 2, 3, 4, 5]) if prop_type != 5 else 0
    bathrooms = max(1, bedrooms)
    area = random.randint(400, 5000)

    type_name = TYPE_NAMES[prop_type]
    listing_type = random.choice(LISTING_TYPES)
    listing_word = "Sale" if listing_type == 0 else "Rent"

    title = f"{type_name} for {listing_word} in {community}"
    if bedrooms > 0:
        title = f"{bedrooms}BR {type_name} in {community}"

    return {
        "title": title,
        "description": f"Beautiful {type_name.lower()} located in the heart of "
                      f"{community}. {fake.sentence(nb_words=15)}",
        "price": price,
        "serviceCharge": round(price * 0.01),
        "location": community,
        "community": community,
        "emirate": emirate,
        "type": prop_type,
        "listingType": listing_type,
        "bedrooms": bedrooms,
        "bathrooms": bathrooms,
        "areaSqFt": area,
        "hasParking": random.choice([True, False]),
        "hasPool": random.choice([True, False]),
        "hasGym": random.choice([True, False]),
        "isFurnished": random.choice([True, False])
    }

def seed_properties(count=100):
    token = get_agent_token()
    if not token:
        return

    headers = {"Authorization": f"Bearer {token}"}

    print(f"\n🏠 Seeding {count} properties...\n")

    success = 0
    failed = 0

    for i in range(count):
        prop_type = random.choice(PROPERTY_TYPES)
        property_data = generate_property(prop_type)

        response = requests.post(
            f"{PROPERTY_URL}/Properties",
            json=property_data,
            headers=headers
        )

        if response.status_code == 201:
            success += 1
            print(f"  ✅ [{i+1:>3}/{count}] {property_data['title'][:50]}")
        else:
            failed += 1
            print(f"  ❌ [{i+1:>3}/{count}] Failed: {response.status_code}")

    print(f"\n{'='*55}")
    print(f"  📊 Seeding complete!")
    print(f"  ✅ Success: {success}")
    print(f"  ❌ Failed:  {failed}")
    print(f"  📁 Total:   {count}")
    print(f"{'='*55}\n")

if __name__ == "__main__":
    seed_properties(100)