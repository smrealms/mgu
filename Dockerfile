FROM mono:latest

COPY ./MGU /MGU
WORKDIR MGU

RUN mono --version
RUN msbuild MGU.sln
RUN mono MGU.exe
