name = Swish
output = bin/Release/
exe = $(output)$(name).exe
source = $(shell find compiler/ -name "*.cs")

all: $(exe)

$(exe): $(source)
	@mkdir -p $(output)
	mcs -debug $(source) -out:$(exe)


run: $(exe)
	mono --debug ./$(exe) Test.sw | tee Test.il
	@ilasm Test.il
	@chmod +x Test.exe
	@./Test.exe

clean:
	rm Test.il
	rm Test.exe
	rm -rf $(output)
