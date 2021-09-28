{{/*
Expand the name of the chart.
*/}}
{{- define "aka-bwhazel.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
The name is truncated at 63 characters because some Kubernetes name fields
are limited to this by the DNS naming spec.  If release name contains chart
name it will be used as the full name.
*/}}
{{- define "aka-bwhazel.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "aka-bwhazel.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels.
*/}}
{{- define "aka-bwhazel.labels" -}}
helm.sh/chart: {{ include "aka-bwhazel.chart" . }}
{{ include "aka-bwhazel.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels.
*/}}
{{- define "aka-bwhazel.selectorLabels" -}}
app.kubernetes.io/name: {{ include "aka-bwhazel.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}