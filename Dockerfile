FROM mono:latest

COPY ./MGU /MGU
WORKDIR MGU

RUN mono --version
RUN msbuild MGU.sln

CMD ["mono", "/MGU/bin/MGU.exe"]
