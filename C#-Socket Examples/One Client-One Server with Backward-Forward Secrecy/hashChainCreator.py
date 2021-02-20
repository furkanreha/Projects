import hashlib
import random

numberHashChains = 2  # Number of hash chains
keySize = 128  # Key size in terms of bits
n = 50  # Length of hash chains

for i in range(numberHashChains):
    file = open("hashChain-" + str(i + 1) + ".txt", "w")
    x = str(hex(random.getrandbits(keySize)))[2:]

    file.write(x + "\n")

    for _ in range(n):
        newHash = str(hashlib.sha3_256(x.encode()).hexdigest())[0:(keySize//4)]
        x = newHash
        file.write(x + "\n")
