file_reader = open("result.txt", "r")
result_data = file_reader.readlines()

file_reader = open("report.txt", "r")
all_file = file_reader.readlines()

bias_freq_data = all_file[5][24:].split("(")
freq_dic = {}
for i in range(0, len(bias_freq_data)):
    array = bias_freq_data[i][0:len(bias_freq_data[i]) - 2].split(" ")
    freq_dic[array[0]] = array[1].replace(")", "")

matching_freq = 0

for e in result_data:
    array = e.split(" ")
    action = array[1].replace("\"", "")
    freq = array[2].replace("\"", "").replace("P", "")
    expected_freq = freq_dic[action]
    if freq == expected_freq:
        matching_freq += 1

print("Frequency Matching Ratio:", "{:.2%}".format(matching_freq / len(result_data)))

bias_period_data = all_file[6][21:].split("(")
period_dic = {}

for i in range(0, len(bias_period_data)):
    array = bias_period_data[i][0:len(bias_period_data[i]) - 2].split(" ")
    period_dic[array[0]] = array[1].replace(")", "")

matching_period = 0

for e in result_data:
    array = e.split(" ")
    action = array[1].replace("\"", "").strip()
    period = array[3].replace("\"", "").strip()
    expected_period = period_dic[action]
    if period == expected_period:
        matching_period += 1

print("Period Matching Ratio:", "{:.2%}".format(matching_period / len(result_data)))
