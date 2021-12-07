FROM node:14.16.1

USER root
ARG APP=app
ARG USER=mint
ARG PWD=mint
ENV HOME=/home/${USER}
WORKDIR ${HOME}/app
COPY $APP .

# APT
RUN apt update
RUN apt upgrade -y
RUN apt install -y git lsof nano sudo zsh

# USER
RUN useradd -d ${HOME} -s $(which zsh) ${USER}
RUN chown -R ${USER} ${HOME}

# ANDROID
# RUN wget https://dl.google.com/android/android-sdk_r24.4.1-linux.tgz -O /opt/android-sdk.tgz
# RUN tar zxvf /opt/android-sdk.tgz -C /opt/
# RUN rm /opt/android-sdk.tgz

# NPM
RUN npm i -g @angular/cli

# ZSH
RUN git clone -b docker --single-branch https://github.com/RaphaelCimaMint/dotfiles.git ${HOME}/tmp
RUN for file in ${HOME}/tmp/.*; do mv $file ${HOME}; done
RUN rm -Rf ${HOME}/tmp

# USER
RUN usermod -aG sudo ${USER}
USER ${USER}

EXPOSE 8100
EXPOSE 35729
EXPOSE 53703
