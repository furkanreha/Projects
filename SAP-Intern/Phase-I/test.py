file_reader = open("sample.txt", "r")
test_writer = open("test.txt", "w")

actual_data = file_reader.readlines()
actual_data.pop(0)

user_action_query = []
for element in actual_data:
    row_array = element.split(" ")
    user_action_query.append(row_array[0] + " " + row_array[1])

frequency = {}
for element in user_action_query:
    if element not in frequency:
        frequency[element] = 1
    else:
        frequency[element] += 1

total = len(frequency)
index = int(input("Enter the Limit Index \nAbove (Average * (Index / 100)) Will Be Accepted: "))
average = (len(user_action_query) / total) * (index / 100)

test_writer.write("id action\n")
counter = 0
for element in frequency.keys():
    if frequency[element] > average:
        test_writer.write(element + "\n")
        counter += 1
print("Test Size:", counter)
