import binascii

lookUpTable = {"0": "0;f", "1": "8;7", "2": "4;b", "3": "c;3", "4": "2;d", "5": "a;5", "6": "6;9", "7": "e;1",
               "8": "1;e", "9": "9;6", "a": "5;a", "b": "d;2", "c": "3;c", "d": "b;4", "e": "7;8", "f": "f;0"}


def preProcesses(message):
    resultingMessage = ""

    numberOfBytes = len(message)
    processedNumberOfBytes = 0
    for i in range(numberOfBytes // 2):
        if processedNumberOfBytes < 8:
            resultingMessage += lookUpTable[message[2 * i + 1]].split(";")[1]
            resultingMessage += lookUpTable[message[2 * i]].split(";")[1]
            processedNumberOfBytes += 2
        elif processedNumberOfBytes < numberOfBytes:
            resultingMessage += lookUpTable[message[2 * i + 1]].split(";")[0]
            resultingMessage += lookUpTable[message[2 * i]].split(";")[0]
            processedNumberOfBytes += 2
        else:
            break
    return resultingMessage


def postProcess(message):
    reverseMessage = message[::-1]
    resultingMessage = ""

    for letter in reverseMessage:
        resultingMessage += lookUpTable[letter].split(";")[1]

    return resultingMessage


print("^ means XOR")
inputMessage = str(input("Enter the message in hex format: ")).lower()
bitPositions = str(input("Enter the flipping bits (Ex Input: 5 7 9): ")).split(" ")
print(40 * "*")

print("Sender does the following steps: \n")

preProcessedMessage = preProcesses(inputMessage)
print("Preprocess the message (M') =", preProcessedMessage)

crcPrime = hex(binascii.crc32(binascii.a2b_hex(preProcessedMessage))).lstrip('0x')
if len(crcPrime) != 8:
    crcPrime = (8 - len(crcPrime)) * "0" + crcPrime

print("CRC'(M') =", crcPrime)

nativeCrc = postProcess(crcPrime)
print("Post Process the output of CRC'; Native CRC (M) =", nativeCrc)

mNativeCrc = (inputMessage + nativeCrc)
print("M|CRC(M) =", mNativeCrc)
k = len(mNativeCrc) * "f"
ciphertext = hex(int(k, 16) ^ int(mNativeCrc, 16)).lstrip('0x')
print("M|CRC(M) ^ k =", ciphertext)
print("Try to send ciphertext(M|CRC(M) ^ k) to receiver..")
print(40 * "*")

print("Attacker does the following steps: \n")

lenMessage = len(inputMessage)
bitSizeOfMessage = (lenMessage * 4)
deltaMBits = bitSizeOfMessage * ["0"]
for flopBit in bitPositions:
    deltaMBits[bitSizeOfMessage - int(flopBit)] = "1"
deltaMBits = ''.join([str(elem) for elem in deltaMBits])
deltaM = hex(int(deltaMBits, 2)).lstrip('0x')

if len(deltaM) != len(inputMessage):
    deltaM = (len(inputMessage) - len(deltaM)) * "0" + deltaM
print("Delta-M =", deltaM)

preProcessedDeltaM = preProcesses(deltaM)
print("Preprocess Delta-M =", preProcessedDeltaM)

crcPrimeDeltaM = hex(binascii.crc32(binascii.a2b_hex(preProcessedDeltaM))).lstrip('0x')
if len(crcPrimeDeltaM) != 8:
    crcPrimeDeltaM = (8 - len(crcPrimeDeltaM)) * "0" + crcPrimeDeltaM
print("CRC'(Delta-M') =", crcPrimeDeltaM)


nativeCrcDeltaM = postProcess(crcPrimeDeltaM)
print("Post Process the output of CRC'; Native CRC(Delta-M) =", nativeCrcDeltaM)

mNativeCrcDeltaM = (deltaM + nativeCrcDeltaM)
xorCiphertext = hex(int(mNativeCrcDeltaM, 16) ^ int(ciphertext, 16)).lstrip('0x')

print("Delta-M|CRC(Delta-M) ^ ciphertext =", xorCiphertext)
print("Sends (Delta-M|CRC(Delta-M) ^ ciphertext) instead of ciphertext(M|CRC(M) ^ k) to receiver..")
print(40 * "*")

print("Receiver does the following steps: \n")

plaintext = hex(int(xorCiphertext, 16) ^ int(k, 16)).lstrip('0x')

if len(plaintext) != len(xorCiphertext):
    plaintext = (len(xorCiphertext) - len(plaintext)) * "0" + plaintext

print("Received Ciphertext from Attacker ^ k =", plaintext)

receivedMessage = plaintext[0:lenMessage]
receivedCrc = plaintext[lenMessage:]
print("Actual Message:", inputMessage, "Received Message:", receivedMessage)
print("Bits in actual and received message to check whether the bits are correctly flipped:")
binInputMessage, binReceivedMessage = bin(int(inputMessage, 16)).lstrip('0b'), \
                                      bin(int(receivedMessage, 16)).lstrip('0b')

if len(binInputMessage) != bitSizeOfMessage:
    binInputMessage = (bitSizeOfMessage - len(binInputMessage)) * "0" + binInputMessage

if len(binReceivedMessage) != bitSizeOfMessage:
    binReceivedMessage = (bitSizeOfMessage - len(binReceivedMessage)) * "0" + binReceivedMessage


print(binInputMessage)
print(binReceivedMessage)

check = True
for i in range(bitSizeOfMessage, 0, -1):
    if str(bitSizeOfMessage - i + 1) in bitPositions:
        if binInputMessage[i - 1] == binReceivedMessage[i - 1]:
            check = False
            break
    else:
        if binInputMessage[i - 1] != binReceivedMessage[i - 1]:
            check = False
            break

if check:
    print("Attacker correctly changes bits")
else:
    print("Attacker fails to changes bits")

print("Now Receiver Does Cross Check..")

preProcessedReceivedMessage = preProcesses(receivedMessage)
print("Preprocess Received Message (Received Message') =", preProcessedReceivedMessage)

crcPrimeReceivedMessage = hex(binascii.crc32(binascii.a2b_hex(preProcessedReceivedMessage))).lstrip('0x')
if len(crcPrimeReceivedMessage) != 8:
    crcPrimeReceivedMessage = (8 - len(crcPrimeReceivedMessage)) * "0" + crcPrimeReceivedMessage
print("CRC'(Received Message') =", crcPrimeReceivedMessage)

nativeCrcReceivedMessage = postProcess(crcPrimeReceivedMessage)
print("Native CRC (Received Message) =", nativeCrcReceivedMessage)
print("Received CRC =", receivedCrc)
if receivedCrc == nativeCrcReceivedMessage:
    print("Since both are equal to each other; Attack Succeeds..")
else:
    print("Since both are not equal to each other; Attack Fails..")

print(40 * "*")
# 54aae0716f
# 5 15 33
# ab0d291aa74d
# 4 6 10 20 24 36
# 879a420b69cc
# 8 10 14 21 33 34 42 48
# a7b449cdf1
# 3 5 14 15 21 24 30
